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

using System.Linq;
using GreenEnergyHub.Ingestion.Synchronous.Infrastructure;
using GreenEnergyHub.Ingestion.Synchronous.Infrastructure.RequestMediation;
using GreenEnergyHub.Ingestion.Synchronous.Infrastructure.RequestRouting;
using GreenEnergyHub.Ingestion.Synchronous.Tests.TestHelpers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GreenEnergyHub.Ingestion.Synchronous.Tests
{
    public class HandlerExtensionsTests
    {
        [Fact]
        public void HandlerExtensionsWithReasonableDefaults_Should_Setup_MediatR()
        {
            const bool validateScopes = true;
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddGreenEnergyHub();

            var serviceProvider = serviceCollection.BuildServiceProvider(validateScopes);
            var actual = serviceProvider.GetService(typeof(IMediator));

            Assert.NotNull(actual);
        }

        [Fact]
        public void HandlerExtension_Should_Locate_One_HubActionHandler()
        {
            const bool validateScopes = true;
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddGreenEnergyHub();

            var serviceProvider = serviceCollection.BuildServiceProvider(validateScopes);
            var requestRegistrations = serviceProvider.GetServices(typeof(RequestRegistration)).Count();

             const int expected = 1;

             Assert.Equal(expected, requestRegistrations);
        }

        [Fact]
        public void HandlerExtension_Should_inject_registrations_into_HubRequestTypeMap()
        {
            const bool validateScopes = true;
            var expectedType = typeof(TestActionRequest);

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddGreenEnergyHub(expectedType.Assembly);

            var serviceProvider = serviceCollection.BuildServiceProvider(validateScopes);
            var requestHub = serviceProvider.GetRequiredService<IHubRequestTypeMap>();

            var actualType = requestHub.GetTypeByCategory(expectedType.Name);

            Assert.NotNull(actualType);
            Assert.Equal(expectedType, actualType);
        }
    }
}
