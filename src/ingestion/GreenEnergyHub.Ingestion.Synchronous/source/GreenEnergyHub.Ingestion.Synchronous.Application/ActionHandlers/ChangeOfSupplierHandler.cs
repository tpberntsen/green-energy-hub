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
using System.Threading;
using System.Threading.Tasks;
using GreenEnergyHub.Ingestion.Synchronous.Infrastructure.RequestTypes;

namespace GreenEnergyHub.Ingestion.Synchronous.Application.ActionHandlers
{
    /// <summary>
    /// Class which defines how to handle ChangeOfSupplierRequests.
    /// </summary>
    public class ChangeOfSupplierHandler : HubActionHandler<ChangeOfSupplierRequest>
    {
        private readonly IRuleEngine<ChangeOfSupplierRequest> _rulesEngine;

        /// <summary>
        /// Builds a ChangeOfSupplierHandler which validates messages using a
        /// provided IRuleEngine.
        /// </summary>
        /// <param name="rulesEngine">The IRuleEngine to validate messages with.
        /// </param>
        public ChangeOfSupplierHandler(IRuleEngine<ChangeOfSupplierRequest> rulesEngine)
        {
            _rulesEngine = rulesEngine;
        }

        /// <summary>
        /// Validates a given ChangeOfSupplierRequest.
        /// </summary>
        /// <param name="actionData">The ChangeOfSupplierRequest.</param>
        /// <returns>True if it is valid.</returns>
        public override async Task<bool> ValidateAsync(ChangeOfSupplierRequest actionData)
        {
            return await _rulesEngine.ValidateAsync(actionData).ConfigureAwait(false);
        }

        /// <summary>
        /// Accepts a ChangeOfSupplierRequest.
        /// </summary>
        /// <param name="actionData">The ChangeOfSupplierRequest.</param>
        /// <returns>True if the request was successfully accepted.</returns>
        public override Task<bool> AcceptAsync(ChangeOfSupplierRequest actionData)
        {
            Console.WriteLine("Writing Service Start Request to DB!");
            return Task.FromResult(true);
        }

        /// <summary>
        /// Creates a response for the ChangeOfSupplierRequest.
        /// </summary>
        /// <param name="actionData">The ChangeOfSupplierRequest.</param>
        /// <returns>A response.</returns>
        public override Task<IHubActionResponse> RespondAsync(ChangeOfSupplierRequest actionData)
        {
            return Task.FromResult<IHubActionResponse>(new HubActionResponse());
        }
    }
}
