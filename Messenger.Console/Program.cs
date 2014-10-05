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

                var req = JsonConvert.DeserializeObject<TransferRequest>(File.ReadAllText(args[0]));
                var serialized = JsonConvert.SerializeObject(req, Formatting.Indented);

                Console.WriteLine(serialized + Environment.NewLine);
                Console.Write("Send ?");

                if (Console.ReadLine().ToUpperInvariant() == "Y")
                {
                    SendRequest(req);
                }

                Console.Read();
            }
            else
            {
                throw new ArgumentException("Provide 0-1 args.");
            }
        }

        private static void SendRequest(TransferRequest request)
        {
            new TransferClient("LOCALHOST -1", null, null).Transfer(request);
        }
    }
}
