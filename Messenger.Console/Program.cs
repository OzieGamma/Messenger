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

    using Messenger.Models;

    using Newtonsoft.Json;

    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length != 1 || !File.Exists(args[0]))
            {
                throw new ArgumentException("Please provide 1 arg, a file name");
            }

            var ompData = File.ReadAllLines(args[0]);
            var meta = ompData[0].Split();
            var message = new string(ompData.Skip(1).Aggregate((acc, text) => acc + '\n' + text).ToArray());

            var req = JsonConvert.SerializeObject(MakeRequest(meta, message));

            Console.WriteLine("Request build: ");
            Console.WriteLine(req);
            File.WriteAllText("out.json", req);


            Console.Write("Send to: ");
            var address = Console.ReadLine();
        }

        private static TransferRequest MakeRequest(IList<string> meta, string message)
        {
            var options = new TransferRequestOptions { Trace = meta[0].ToUpperInvariant() == "TRACE" };
            var algorithm = meta[1];
            var times = int.Parse(meta[2]);
            var endProtocol = meta[3].Split(':')[0];
            var endTarget = new TransferTarget { To = meta[3].Split(':')[1] };

            var lastRequest = new TransferRequest
                              {
                                  Options = options, 
                                  Payload = Encrypt(message, algorithm, endTarget), 
                                  To = endTarget.To, 
                                  Algorithm = algorithm, 
                                  Protocol = ParseProtocol(endProtocol)
                              };

            return BundleRequest(options, algorithm, times, lastRequest);
        }

        private static string Encrypt(string message, string algorithm, TransferTarget target)
        {
            switch (algorithm)
            {
                case "clear_text":
                    return message;
                default:
                    throw new NotImplementedException("Don't know algorithm: " + algorithm);
            }
        }

        private static TransferRequest BundleRequest(TransferRequestOptions options, string algorithm, int times, TransferRequest bundledRequest)
        {
            while (true)
            {
                if (times == 0)
                {
                    return bundledRequest;
                }

                var payload = JsonConvert.SerializeObject(bundledRequest);
                var target = RandomTarget();

                var message = new TransferRequest { Options = options, Payload = Encrypt(payload, algorithm, target), To = target.To, Algorithm = algorithm, Protocol = TransferRequestProtocol.Omp };

                times = times - 1;
                bundledRequest = message;
            }
        }

        private static TransferTarget RandomTarget()
        {
            return new TransferTarget { UID = "LOCALHOST 0", To = "127.0.0.1" };
        }

        private static TransferRequestProtocol ParseProtocol(string protocol)
        {
            TransferRequestProtocol res;
            if (!Enum.TryParse(protocol, true, out res))
            {
                throw new InvalidOperationException("Not recognized protocol: " + protocol);
            }

            return res;
        }
    }
}
