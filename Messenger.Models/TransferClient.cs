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

    using Newtonsoft.Json;

    using SendGrid;

    public class TransferClient
    {
        private readonly string me;
        private readonly string mailName;
        private readonly string mailPass;

        public TransferClient(string me, string mailName, string mailPass)
        {
            this.me = me;
            this.mailName = mailName;
            this.mailPass = mailPass;
        }

        public void Transfer(IncomingRequest req)
        {
            switch (req.Algorithm)
            {
                case TransferRequestAlgorithm.ClearText:
                    this.TransferClearText(req);
                    break;
                case TransferRequestAlgorithm.Rsa4096:
                    this.TransferRsa4096(req);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void Transfer(TransferRequest req)
        {
            if (req.ShouldStamp)
            {
                req.Payload.Trace.Add(this.MakeStamp());
            }

            switch (req.Protocol)
            {
                case TransferRequestProtocol.Omp:
                    this.TransferOmp(req);
                    break;
                case TransferRequestProtocol.Email:
                    this.TransferEmail(req);
                    break;
                case TransferRequestProtocol.Sms:
                    this.TransferSms(req);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void TransferClearText(IncomingRequest req)
        {
            var sendReq = JsonConvert.DeserializeObject<TransferRequest>(req.SendRequest);
            sendReq.Payload.Trace.AddRange(req.Trace);

            this.Transfer(sendReq);
        }

        private void TransferRsa4096(IncomingRequest req)
        {
            throw new NotImplementedException();
        }

        private async void TransferOmp(TransferRequest req)
        {
            var message = JsonConvert.SerializeObject(req.Payload);

            var content = new StringContent(message, Encoding.UTF8, "application/json");
            var res = await new HttpClient().PostAsync(req.To, content);

            Console.WriteLine(res.StatusCode);
        }

        private string MakeStamp()
        {
            return string.Format("Transfered by {0} @ {1} {2}", this.me, DateTime.Now, Environment.NewLine);
        }

        private void TransferEmail(TransferRequest req)
        {
            // Create the email object first, then add the properties.
            var email = new SendGridMessage
                        {
                            From =
                                new MailAddress("noreply@anonymous.com", "Your willing postmaster."),
                            Subject = "Ping! You got a message !",
                            Text = JsonConvert.SerializeObject(req.Payload)
                        };

            email.AddTo(req.To);


            // Create an Web transport for sending email.
            var transportWeb = new Web(new NetworkCredential(this.mailName, this.mailPass));

            // Send the email.
            transportWeb.DeliverAsync(email);
        }

        private void TransferSms(TransferRequest req)
        {
            throw new NotImplementedException(req.ToString());
        }
    }
}
