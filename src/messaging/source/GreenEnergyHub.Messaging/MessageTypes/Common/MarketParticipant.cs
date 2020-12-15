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
using System.Text.Json.Serialization;

namespace GreenEnergyHub.Messaging.MessageTypes.Common
{
    public class MarketParticipant
    {
        public MarketParticipant()
            : this(MRID.Empty, null, null) { }

        public MarketParticipant(string mrid)
            : this(new MRID(mrid), null, null)
        { }

        public MarketParticipant(MRID mrid, string? name, string? type)
        {
            MRID = mrid;
            Name = name;
            Type = type;
        }

        public static MarketParticipant Empty => new MarketParticipant();

        [JsonPropertyName(name: "mRID")]
        public MRID MRID { get; set; }

        [JsonPropertyName(name: "name")]
        public string? Name { get; set; }

        /// <summary>
        /// Process Role information for market participant, eg. MDR.
        /// </summary>
        public string? Type { get; set; }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == typeof(MarketParticipant) && Equals((MarketParticipant)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MRID, Name);
        }

        protected bool Equals(MarketParticipant other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return MRID.Equals(other.MRID) && Name == other.Name;
        }
    }
}
