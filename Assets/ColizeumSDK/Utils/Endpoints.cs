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

namespace ColizeumSDK.Utils
{
    public static class Endpoints
    {
        public const string Token = Constants.Issuer + "/token";
        public const string RevokeToken = Constants.Issuer + "/token/revocation";
        
        public const string UserInfo = Constants.ApiUrl + "/users/me";

        public const string GetEnergy = Constants.ApiUrl + "/funds/energy";
        public const string ConsumeEnergy = Constants.ApiUrl + "/funds/energy/consume";

        public const string GetSecondaryCurrency = Constants.ApiUrl + "/funds/secondary_currency";
        public const string GetEarnings = Constants.ApiUrl + "/funds/earnings";
    }
}