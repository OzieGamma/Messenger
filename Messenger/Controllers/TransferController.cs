﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransferController.cs" company="Oswald Maskens">
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

namespace Messenger.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;

    using Messenger.Models;

    using Newtonsoft.Json;

    public class TransferController : ApiController
    {
        public async Task<string> Post()
        {
            var json = await Request.Content.ReadAsStringAsync();

            var req = JsonConvert.DeserializeObject<IncomingRequest>(json);
            new TransferClient(My.Name, My.SendGridUsername, My.SendGridPassword).Transfer(req);

            return "ok";
        }
    }
}
