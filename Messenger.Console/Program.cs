// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Oswald Maskens">
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

namespace Messenger.Console
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Messenger.Models;

    internal class Program
    {
        private static void Main(string[] args)
        {
            DefineMy();

            var fromFile = File.ReadAllLines("input.inp").Where(_ => !_.StartsWith("#"));
            var enumerable = fromFile as IList<string> ?? fromFile.ToList();

            var phoneNumbers = enumerable.Where(_ => _.StartsWith("+")).ToList();
            var emailRecipients = enumerable.Where(_ => _.Contains("@")).Where(_ => !string.IsNullOrWhiteSpace(_));

            var emails =
                emailRecipients.Select(
                    _ =>
                    new TransferRequest
                    {
                        FinalProtocol = TransferRequestProtocol.Email, 
                        FinalTo = _, 
                        Payload = @"This is my ap for the 2014 fall Jackobshack hackathon. 
                        I think azure is cool and to think I can send messages in the cloud for free is amazing. 
                        I highly recommend http://azure.microsoft.com/. 
                        All the code for my project is released under the apache2 license(http://www.apache.org/licenses/LICENSE-2.0.html) and can be found at https://github.com/oziegamma. 
                        If you are ever interested in a project / creating a company contact me at oswald@maskens.eu, I'm always up for a geeky adventure. 
                        I hope you all had a nice day. I had a lot of fun playing with VM-farms", 
                        RedPill = MyRandom.RedPill(100, 5000), 
                        ShouldStamp = true, 
                        Trace = new List<string>()
                    });
            var smses =
                phoneNumbers.Select(
                    _ =>
                    new TransferRequest
                    {
                        FinalProtocol = TransferRequestProtocol.Sms, 
                        FinalTo = _, 
                        Payload =
                            @"OzieGamma's App for Jackobshack 2014. Let's keep in touch at oswald@maskens.eu", 
                        RedPill = MyRandom.RedPill(2, 2), 
                        ShouldStamp = false, 
                        Trace = new List<string>()
                    });

            var phoneCalls =
                phoneNumbers.Select(
                    _ =>
                    new TransferRequest
                    {
                        FinalProtocol = TransferRequestProtocol.Call, 
                        FinalTo = _, 
                        Payload =
                            @"Hey there, this is oswald maskens, I'm calling to tell you this is my App for Jackobshack 2014. 
                            I hope you had a good time, I certainly did. Don't hesitate to contact me if you have a cool project ! 
                            The easiest way to reach me is email: oswald@maskens.eu. 
                            You can also find me on Facebook, GitHub, Stackoverflow and LinkedIn.", 
                        RedPill = MyRandom.RedPill(2, 2), 
                        ShouldStamp = false, 
                        Trace = new List<string>()
                    });

            var transfers = new List<TransferRequest>(emails.ToList());
            transfers.AddRange(smses);
            transfers.AddRange(phoneCalls);

            Parallel.ForEach(transfers, _ => new TransferClient().Transfer(_));

            Console.Read();
        }

        private static void DefineMy()
        {
            My.Name = "LOCAL";
            My.Url = "http://localhost";

            My.SendGridPassword = string.Empty;
            My.SendGridUsername = string.Empty;

            My.TwilioSid = string.Empty;
            My.TwilioAuthToken = string.Empty;
            My.TwilioPhoneNumber = string.Empty;
        }

        private static void SendRequest(TransferRequest request)
        {
            new TransferClient().Transfer(request);
        }
    }
}
