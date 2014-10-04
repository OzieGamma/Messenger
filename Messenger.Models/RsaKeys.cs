// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RsaKeys.cs" company="Oswald Maskens">
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
    using System.Security.Cryptography;

    public class RsaKeys
    {
        private readonly RSAParameters privateKey;
        private readonly RSAParameters publicKey;

        public RsaKeys(RSAParameters privateKey, RSAParameters publicKey)
        {
            this.privateKey = privateKey;
            this.publicKey = publicKey;
        }

        public RSAParameters Public
        {
            get
            {
                return this.publicKey;
            }
        }

        public RSAParameters Private
        {
            get
            {
                return this.privateKey;
            }
        }
    }
}
