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
using ColizeumSDK.Models;

using static ColizeumSDK.API.Responses.UserResponse;

namespace ColizeumSDK.Factories
{
    public static class UserFactory
    {
        public static User Create(UserItem userItem)
        {
            var user = new User
            {
                id = userItem.id,
                username = userItem.username,
                email = userItem.email,
                avatar = userItem.avatar,

                bio = userItem.bio,
                twitter = userItem.twitter,
                discord = userItem.discord,
                telegram = userItem.telegram,

                createdAt = userItem.created_at,

                energy = EnergyFactory.Create(userItem.energy),
                secondaryCurrency = SecondaryCurrencyFactory.Create(userItem.secondaryCurrency)
            };

            if (userItem.wallets != null)
            {
                user.wallets = new List<Wallet>(userItem.wallets.Select(WalletFactory.Create));
            }

            return user;
        }
    }
}