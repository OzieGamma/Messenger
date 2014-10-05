// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransferClient.cs" company="Oswald Maskens">
//   Copyright 2014 Oswald Maskens
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//   
//       http://www.apache.org/licenses/LICENSE-2.0
//   
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Messenger.Models
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Mail;
    using System.Text;
    using System.Web;

    using Newtonsoft.Json;

    using SendGrid;

    using Twilio;

    public class TransferClient
    {
        public const double ExitChance = 0.0005;

        public void Transfer(TransferRequest req)
        {
            if (req.ShouldStamp)
            {
                req.Trace.Add(this.MakeStamp());
            }

            if (req.RedPill <= 0 || MyRandom.Try(ExitChance))
            {
                switch (req.FinalProtocol)
                {
                    case TransferRequestProtocol.Email:
                        this.TransferEmail(req);
                        break;
                    case TransferRequestProtocol.Sms:
                        this.TransferSms(req);
                        break;
                    case TransferRequestProtocol.Call:
                        this.TransferCall(req);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            else
            {
                req.RedPill -= 1;
                this.ExecuteTransfer(req);
            }
        }

        private async void ExecuteTransfer(TransferRequest req)
        {
            var message = JsonConvert.SerializeObject(req);

            var content = new StringContent(message, Encoding.UTF8, "application/json");
            var res = await new HttpClient().PostAsync(TargetFinder.Random() + "/transfer", content);

            Console.WriteLine(res.StatusCode);
        }

        private string MakeStamp()
        {
            return string.Format("Transfered by {0} @ {1}", My.Name, DateTime.Now);
        }

        private void TransferEmail(TransferRequest req)
        {
            // Create the email object first, then add the properties.
            var email = new SendGridMessage
                        {
                            From =
                                new MailAddress("noreply@anonymous.com", "Your willing postmaster."), 
                            Subject = "Ping! You got a message !", 
                            Text = JsonConvert.SerializeObject(req, Formatting.Indented)
                        };

            email.AddTo(req.FinalTo);

            // Create an Web transport for sending email.
            var transportWeb = new Web(new NetworkCredential(My.SendGridUsername, My.SendGridPassword));

            // Send the email.
            transportWeb.DeliverAsync(email);
        }

        private void TransferSms(TransferRequest req)
        {
            var client = new TwilioRestClient(My.TwilioSid, My.TwilioAuthToken);

            // Send an SMS message.
            client.SendSmsMessage(My.TwilioPhoneNumber, req.FinalTo, req.Payload);
        }

        private void TransferCall(TransferRequest req)
        {
            var client = new TwilioRestClient(My.TwilioSid, My.TwilioAuthToken);
            var url = string.Format(
                "http://twimlets.com/message?Message%5B0%5D={0}", 
                HttpUtility.UrlEncode(req.Payload));

            var options = new CallOptions { From = My.TwilioPhoneNumber, To = req.FinalTo, Url = url };
            client.InitiateOutboundCall(options);
        }
    }
}
