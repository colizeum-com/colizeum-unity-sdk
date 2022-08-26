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
using System.Collections.Generic;
using System.Linq;
using ColizeumSDK.API.Responses;
using ColizeumSDK.Factories;

namespace ColizeumSDK.Models
{
    /// <summary>
    /// Holds information related to user energy and NFTs
    /// </summary>
    [Serializable]
    public class Energy
    {
        /// <summary>
        /// Currently available energy the User has
        /// </summary>
        public int current;

        /// <summary>
        /// Maximum available energy the User has
        /// </summary>
        public int max;

        public List<Token> tokens;

        /// <summary>
        /// Consumes user energy from a specific token (if provided)
        /// </summary>
        /// <param name="amount">Energy amount that will be consumed</param>
        /// <param name="tokenId">Token ID from which the energy will be consumed (optional)</param>
        /// <param name="onSuccess"></param>
        /// <param name="onError"></param>
        public void Consume(int amount, string tokenId = null, Action<ConsumeEnergyResponse> onSuccess = null,
            Action<Exception> onError = null)
        {
            Colizeum.API.ConsumeUserEnergy(amount, tokenId, response =>
            {
                current = response.item.remaining_energy;

                onSuccess?.Invoke(response);
            }, onError);
        }

        /// <summary>
        /// Fetches latest information from the API and updates the model
        /// </summary>
        /// <param name="onSuccess"></param>
        public void Refresh(Action onSuccess = null)
        {
            Colizeum.API.GetUserEnergy(response =>
            {
                current = response.item.total_energy;
                max = response.item.max_energy;

                tokens = new List<Token>(response.item.tokens.Select(TokenFactory.Create));

                onSuccess?.Invoke();
            });
        }
    }
}