﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TargetFinder.cs" company="Oswald Maskens">
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

    internal static class TargetFinder
    {
        private static readonly IList<string> Targets;

        private static readonly Random Rand = new Random();

        static TargetFinder()
        {
            Targets =
                new[] { 1, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }.Select(
                    _ => "https://messenger-" + _ + ".azurewebsites.net").ToList();
        }

        public static string Random()
        {
#if DEBUG
            return "http://localhost:64107";
#else
            return Targets[Rand.Next(0, Targets.Count)];
#endif
        }
    }
}
