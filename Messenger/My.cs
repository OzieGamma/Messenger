// --------------------------------------------------------------------------------------------------------------------
// <copyright file="My.cs" company="Oswald Maskens">
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

namespace Messenger
{
    using System.Configuration;

    using Messenger.Models;

    internal static class My
    {
        internal static string Name
        {
            get
            {
                return ConfigurationManager.AppSettings["UID"];
            }
        }

        internal static string SendGridUsername
        {
            get
            {
                return ConfigurationManager.AppSettings["Sendgrid_username"];
            }
        }

        internal static string SendGridPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["Sendgrid_pwd"];
            }
        }
    }
}
