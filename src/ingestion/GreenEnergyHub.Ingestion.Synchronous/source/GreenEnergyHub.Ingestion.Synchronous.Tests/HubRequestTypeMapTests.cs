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
using GreenEnergyHub.Ingestion.Synchronous.Infrastructure;
using GreenEnergyHub.Ingestion.Synchronous.Infrastructure.RequestMediation;
using GreenEnergyHub.Ingestion.Synchronous.Infrastructure.RequestRouting;
using GreenEnergyHub.Ingestion.Synchronous.Tests.TestHelpers;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace GreenEnergyHub.Ingestion.Synchronous.Tests
{
    public class HubRequestTypeMapTests
    {
        private readonly AutoMocker _autoMocker;

        public HubRequestTypeMapTests()
        {
            _autoMocker = new AutoMocker(MockBehavior.Default);
        }

        [Fact]
        public void ValidCategory_ReturnsCorrectType()
        {
            var requestMaps = new[] { new RequestRegistration("ServiceStart", typeof(Type)) };
            var hubRequestTypeMap = new HubRequestTypeMap(requestMaps);

            var result = hubRequestTypeMap.GetTypeByCategory("ServiceStart");
            Assert.Equal(typeof(Type), result);
        }

        [Fact]
        public void InvalidCategory_ReturnsNull()
        {
            var requestMaps = Array.Empty<RequestRegistration>();
            var hubRequestTypeMap = new HubRequestTypeMap(requestMaps);

            var result = hubRequestTypeMap.GetTypeByCategory("ServiceStart");
            Assert.Null(result);
        }
    }
}
