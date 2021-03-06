﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="Oswald Maskens">
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
    using System.Web;
    using System.Web.Http;

    using Messenger.Models;

    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            My.Name = ConfigurationManager.AppSettings["UID"];
            My.Url = ConfigurationManager.AppSettings["Url"];

            My.SendGridUsername = ConfigurationManager.AppSettings["Sendgrid_username"];
            My.SendGridPassword = ConfigurationManager.AppSettings["Sendgrid_pwd"];

            My.TwilioSid = ConfigurationManager.AppSettings["Twilio_sid"];
            My.TwilioAuthToken = ConfigurationManager.AppSettings["Twilio_auth_token"];
            My.TwilioPhoneNumber = ConfigurationManager.AppSettings["Twilio_phonenumber"];

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
