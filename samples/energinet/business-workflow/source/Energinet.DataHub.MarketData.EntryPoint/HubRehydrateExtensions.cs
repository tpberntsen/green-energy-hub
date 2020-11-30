﻿// Copyright 2020 Energinet DataHub A/S
//
// Licensed under the Apache License, Version 2.0 (the "License2");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.IO;
using System.Threading.Tasks;
using GreenEnergyHub.Messaging;

namespace Energinet.DataHub.MarketData.EntryPoint
{
    internal static class HubRehydrateExtensions
    {
        internal static async Task<IHubRequest?> RehydrateAsync(this IHubRehydrate rehydrate, string input, Type messageType)
        {
            if (rehydrate == null)
            {
                throw new ArgumentNullException(nameof(rehydrate));
            }

            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (messageType == null)
            {
                throw new ArgumentNullException(nameof(messageType));
            }

            await using (var stream = new MemoryStream())
            await using (var writer = new StreamWriter(stream))
            {
                await writer.WriteAsync(input).ConfigureAwait(false);
                await writer.FlushAsync().ConfigureAwait(false);
                stream.Position = 0;
                return await rehydrate.RehydrateAsync(stream, messageType).ConfigureAwait(false);
            }
        }
    }
}
