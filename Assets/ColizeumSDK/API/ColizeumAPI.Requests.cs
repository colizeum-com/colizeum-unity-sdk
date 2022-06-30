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

namespace ColizeumSDK.API
{
    using Requests;
    using Responses;

    public partial class ColizeumAPI
    {
        /// <summary>
        /// Returns user tokens from the provided authorization code
        /// </summary>
        /// <param name="code"></param>
        /// <param name="codeVerifier"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onError"></param>
        public void GetTokensFromCode(string code, string codeVerifier, Action<TokenResponse> onSuccess,
            Action<Exception> onError = null)
        {
            Post(new GetTokenRequest(code, codeVerifier), onSuccess, onError);
        }

        /// <summary>
        /// Tries to fetch a new access token using the existing refresh token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onError"></param>
        public void RefreshAccessToken(string refreshToken, Action<TokenResponse> onSuccess,
            Action<Exception> onError = null)
        {
            Post(new RefreshAccessTokenRequest(refreshToken), onSuccess, onError);
        }

        /// <summary>
        /// Revokes provided token, making it unusable
        /// </summary>
        /// <param name="token"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onError"></param>
        public void RevokeToken(string token, Action<GenericResponse> onSuccess = null,
            Action<Exception> onError = null)
        {
            Post(new RevokeTokenRequest(token), onSuccess, onError);
        }

        /// <summary>
        /// Returns user information
        /// </summary>
        /// <param name="onSuccess"></param>
        /// <param name="onError"></param>
        public void GetMe(Action<UserResponse> onSuccess, Action<Exception> onError = null)
        {
            Get(new GetMeRequest(), onSuccess, onError);
        }

        /// <summary>
        /// Returns available energy amount which user has
        /// </summary>
        /// <param name="onSuccess"></param>
        /// <param name="onError"></param>
        public void GetUserEnergy(Action<GetEnergyResponse> onSuccess, Action<Exception> onError = null)
        {
            Get(new GetEnergyRequest(), onSuccess, onError);
        }

        /// <summary>
        /// Consumes the specified amount of energy. If tokenId is provided, tries to consume energy for that specific token
        /// </summary>
        /// <param name="tokenId"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onError"></param>
        /// <param name="amount"></param>
        public void ConsumeUserEnergy(int amount, string tokenId, Action<ConsumeEnergyResponse> onSuccess,
            Action<Exception> onError = null)
        {
            Post(new ConsumeEnergyRequest(amount, tokenId), onSuccess, onError);
        }

        /// <summary>
        /// Returns currently available secondary currency the user has in Colizeum platform
        /// </summary>
        /// <param name="onSuccess"></param>
        /// <param name="onError"></param>
        public void GetSecondaryCurrency(Action<GetSecondaryCurrencyResponse> onSuccess,
            Action<Exception> onError = null)
        {
            Get(new GetSecondaryCurrencyRequest(), onSuccess, onError);
        }

        /// <summary>
        /// Returns secondary earnings for this specific game
        /// </summary>
        /// <param name="onSuccess"></param>
        /// <param name="onError"></param>
        public void GetEarnings(Action<GetEarningsResponse> onSuccess, Action<Exception> onError = null)
        {
            Get(new GetEarningsRequest(), onSuccess, onError);
        }
    }
}