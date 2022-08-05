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

using System.Collections.Generic;
using System.Linq;
using ColizeumSDK.API.Responses;
using ColizeumSDK.Models;

namespace ColizeumSDK.Factories
{
    public static class EnergyFactory
    {
        public static Energy Create(GetEnergyResponse.EnergyItem energyItem)
        {
            var energy = new Energy
            {
                current = energyItem.total_energy,
                total = energyItem.max_energy,
            };

            if (energyItem.tokens != null)
            {
                energy.tokens = new List<Token>(energyItem.tokens.Select(TokenFactory.Create));
            }

            return energy;
        }
    }
}