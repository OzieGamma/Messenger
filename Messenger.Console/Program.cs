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
    using System.Linq;
    using System.Threading.Tasks;

    using Messenger.Models;

    internal class Program
    {
        private static void Main()
        {
            DefineMy();

            var recipients = new string[0]; // define who will recieve your message

            var transfers =
                recipients.Select(
                    _ =>
                    new TransferRequest
                    {
                        FinalProtocol = TransferRequestProtocol.Email, 
                        FinalTo = _, 
                        Payload = @"Lorem ipsum", // define your message 
                        RedPill = MyRandom.RedPill(100, 5000), 
                        ShouldStamp = true, 
                        Trace = new List<string>()
                    });

            // Run it !!!
            Parallel.ForEach(transfers, SendRequest);

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
