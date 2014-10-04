// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransferTarget.cs" company="Oswald Maskens">
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
    using System.Net.Http;
    using System.Security.Cryptography;

    using Newtonsoft.Json;

    public class TransferTarget
    {
        private RSAParameters? publicKey;

        public string UID { get; set; }

        public string To { get; set; }

        public RSAParameters PublicKey
        {
            get
            {
                if (this.publicKey == null)
                {
                    this.publicKey =
                        JsonConvert.DeserializeObject<RSAParameters>(
                            new HttpClient().GetAsync(this.To + "/key").Result.Content.ReadAsStringAsync().Result);
                }

                return this.publicKey.GetValueOrDefault();
            }
        }
    }
}
