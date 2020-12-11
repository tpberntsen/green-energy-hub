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
using Energinet.DataHub.Ingestion.Application.Messages;
using GreenEnergyHub.Messaging;
using GreenEnergyHub.Messaging.Dispatching;
using GreenEnergyHub.Messaging.MessageQueue;

namespace Energinet.DataHub.Ingestion.Application.Handlers
{
    /// <summary>
    /// Class which defines how to handle TimeSeriesMessages
    /// for the synchronous ingestor.
    /// </summary>
    public class TimeSeriesRequestHandler : HubRequestHandler<TimeSeriesMessage>
    {
        private readonly IRuleEngine<TimeSeriesMessage> _rulesEngine;
        private readonly IHubMessageQueueDispatcher _messageDispatcher;

        /// <summary>
        /// Builds a TimeSeriesRequestHandler which validates messages using a
        /// provided IRuleEngine and dispatches valid messages to a queue.
        /// </summary>
        /// <param name="rulesEngine">The IRuleEngine to validate messages with.
        /// </param>
        /// <param name="messageQueueDispatcher">Queue dispatcher to use when request is successfully validated.</param>
        public TimeSeriesRequestHandler(
            IRuleEngine<TimeSeriesMessage> rulesEngine,
            IHubMessageQueueDispatcher messageQueueDispatcher)
        {
            _rulesEngine = rulesEngine;
            _messageDispatcher = messageQueueDispatcher;
        }

        /// <summary>
        /// Validates a given TimeSeriesMessage.
        /// </summary>
        /// <param name="actionData">The TimeSeriesMessage.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>True if it is valid.</returns>
        protected override Task<bool> ValidateAsync(TimeSeriesMessage actionData, CancellationToken cancellationToken)
        {
            // TODO: Enable validation when we are settled on a validation engine/methodology
            // return await _rulesEngine.ValidateAsync(actionData).ConfigureAwait(false);
            return Task.FromResult(true);
        }

        /// <summary>
        /// Accepts a TimeSeriesMessage.
        /// </summary>
        /// <param name="actionData">The TimeSeriesMessage.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>True if the request was successfully accepted.</returns>
        protected override async Task<bool> AcceptAsync(TimeSeriesMessage actionData, CancellationToken cancellationToken)
        {
            await _messageDispatcher.DispatchAsync(actionData).ConfigureAwait(false);
            return true;
        }

        /// <summary>
        /// Creates a response for the TimeSeriesMessage.
        /// </summary>
        /// <param name="actionData">The TimeSeriesMessage.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A response.</returns>
        protected override Task<IHubResponse> RespondAsync(TimeSeriesMessage actionData, CancellationToken cancellationToken)
        {
            return Task.FromResult<IHubResponse>(new HubResponse());
        }
    }
}
