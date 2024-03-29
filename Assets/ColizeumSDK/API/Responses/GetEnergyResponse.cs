/**
 * Copyright (c) 2022-present, Colizeum Association
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the
 * License. You may obtain a copy of the License at
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "
 * AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific
 * language governing permissions and limitations under the License.
 */

using System;

namespace ColizeumSDK.API.Responses
{
    [Serializable]
    public struct GetEnergyResponse
    {
        [Serializable]
        public struct EnergyToken
        {
            public string token_id;
            public int energy;
            public int max_energy;
            public string next_energy_at;
            public int energy_regeneration_amount;
            public int energy_regeneration_rate;
        }

        [Serializable]
        public struct EnergyItem
        {
            public int total_energy;
            public int max_energy;
            public EnergyToken[] tokens;
        }

        public EnergyItem item;
    }
}
