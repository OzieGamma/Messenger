// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Crypto.cs" company="Oswald Maskens">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    public static class Crypto
    {
        private const int BitLength = 2048;
        private const int MaxBlockSize = 244;

        public static RsaKeys CreateKeys()
        {
            using (var rsa = new RSACryptoServiceProvider(BitLength))
            {
                return new RsaKeys(privateKey: rsa.ExportParameters(true), publicKey: rsa.ExportParameters(false));
            }
        }

        public static string Encrypt(string message, TransferRequestAlgorithm algorithm, TransferTarget target)
        {
            switch (algorithm)
            {
                case TransferRequestAlgorithm.ClearText:
                    return message;
                case TransferRequestAlgorithm.Rsa:
                    return EncryptRsa(message, target);
                default:
                    throw new NotImplementedException("Don't know algorithm: " + algorithm);
            }
        }

        public static string Decrypt(string message, TransferRequestAlgorithm algorithm, RSAParameters keys)
        {
            switch (algorithm)
            {
                case TransferRequestAlgorithm.ClearText:
                    return message;
                case TransferRequestAlgorithm.Rsa:
                    return DecryptRsa(message, keys);
                default:
                    throw new NotImplementedException("Don't know algorithm: " + algorithm);
            }
        }

        private static string EncryptRsa(string message, TransferTarget target)
        {
            var dataToEncrypt = Encoding.UTF8.GetBytes(message);
            var block = new byte[MaxBlockSize];
            var blockNumber = 0;
            var sb = new StringBuilder();

            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(target.PublicKey);

                var blockLength = Math.Min(MaxBlockSize, dataToEncrypt.Length - (MaxBlockSize * blockNumber));
                while (blockLength > 0)
                {
                    if (blockLength == MaxBlockSize)
                    {
                        Array.Copy(dataToEncrypt, block, blockLength);
                    }
                    else
                    {
                        block = dataToEncrypt.Skip(dataToEncrypt.Length - blockLength).ToArray();
                    }

                    var subRes = Convert.ToBase64String(rsa.Encrypt(block, true));
                    sb.Append(subRes).Append(':');

                    blockNumber += 1;

                    blockLength = Math.Min(MaxBlockSize, dataToEncrypt.Length - (MaxBlockSize * blockNumber));
                }
            }

            return sb.ToString();
        }

        private static string DecryptRsa(string message, RSAParameters keys)
        {
            var blocks = message.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            var result = blocks.SelectMany(_ => DecryptBlock(_, keys)).ToArray();
            return Encoding.UTF8.GetString(result);
        }

        private static IEnumerable<byte> DecryptBlock(string message, RSAParameters keys)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(keys);

                return rsa.Decrypt(Convert.FromBase64String(message), true);
            }
        }
    }
}
