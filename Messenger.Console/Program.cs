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

    using Messenger.Models;

    using Newtonsoft.Json;

    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                if (!File.Exists(args[0]))
                {
                    throw new ArgumentException("If an arg is provided it should be a file name");
                }

                var genReq = JsonConvert.DeserializeObject<OmpRequestGenerationParameters>(File.ReadAllText(args[0]));
                var req = GenerateRequest(genReq);

                File.WriteAllText("req.json", JsonConvert.SerializeObject(req));

                Console.Write("Send ?");

                if (Console.ReadLine().ToUpperInvariant() == "Y")
                {
                    SendRequest(req);
                }

                Console.Read();
            }
            else if (args.Length == 0)
            {
                var genReq = OmpRequestGenerationParameters.DummyRequest();
                File.WriteAllText("genreq.json", JsonConvert.SerializeObject(genReq));
            }
            else
            {
                throw new ArgumentException("Provide 0-1 args.");
            }
        }

        private static TransferRequest GenerateRequest(OmpRequestGenerationParameters genReq)
        {
            if (genReq.Randoms <= 0)
            {
                return new TransferRequest
                       {
                           Payload =
                               new IncomingRequest
                               {
                                   Algorithm = TransferRequestAlgorithm.ClearText, 
                                   SendRequest = genReq.Message, 
                                   Trace = new List<string>()
                               }, 
                           Protocol = genReq.FinalProtocol, 
                           ShouldStamp = genReq.ShouldStamp, 
                           To = genReq.FinalTo
                       };
            }

            var innerReq = GenerateRequest(genReq.Next());
            var msg = JsonConvert.SerializeObject(innerReq);
            var encrypted = Crypto.Encrypt(msg, genReq.Algorithm, genReq.Target);

            var incomingReq = new IncomingRequest
                              {
                                  Algorithm = genReq.Algorithm, 
                                  SendRequest = encrypted, 
                                  Trace = new List<string>()
                              };

            return new TransferRequest
                   {
                       Payload = incomingReq, 
                       Protocol = TransferRequestProtocol.Omp, 
                       ShouldStamp = genReq.ShouldStamp, 
                       To = genReq.Target.To
                   };
        }

        private static void SendRequest(TransferRequest request)
        {
            new TransferClient("LOCALHOST -1").Transfer(request);
        }
    }
}
