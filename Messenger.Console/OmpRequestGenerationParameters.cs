// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OmpRequestGenerationParameters.cs" company="Oswald Maskens">
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
    using Messenger.Models;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    internal class OmpRequestGenerationParameters
    {
        private TransferTarget target;

        [JsonProperty("should_stamp")]
        public bool ShouldStamp { get; set; }

        [JsonProperty("algorithm")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TransferRequestAlgorithm Algorithm { get; set; }

        [JsonProperty("randoms")]
        public int Randoms { get; set; }

        [JsonProperty("final_protocol")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TransferRequestProtocol FinalProtocol { get; set; }

        [JsonProperty("final_to")]
        public string FinalTo { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonIgnore]
        public TransferTarget Target
        {
            get
            {
                if (this.target == null)
                {
                    this.target = TargetFinder.Random();
                }

                return this.target;
            }
        }

        public static OmpRequestGenerationParameters DummyRequest()
        {
            return new OmpRequestGenerationParameters
                   {
                       Algorithm = TransferRequestAlgorithm.ClearText, 
                       FinalProtocol = TransferRequestProtocol.Email, 
                       FinalTo = "me@example.com", 
                       Message = "Hello, world !", 
                       Randoms = 42, 
                       ShouldStamp = true
                   };
        }

        public OmpRequestGenerationParameters Next()
        {
            return new OmpRequestGenerationParameters
                   {
                       Algorithm = this.Algorithm, 
                       FinalProtocol = this.FinalProtocol, 
                       FinalTo = this.FinalTo, 
                       Message = this.Message, 
                       Randoms = this.Randoms - 1, 
                       ShouldStamp = this.ShouldStamp
                   };
        }
    }
}
