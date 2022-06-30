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
    /// <summary>
    /// Holds values which barely ever change
    /// </summary>
    public static class Constants
    {
        public const string ApiUrl = "https://api.colizeum.com";
        public const string Issuer = "https://identity.colizeum.com";

        // We want this to be in the range of 49152 to 65535, as those are dynamic ports and usually used temporary 
        public const int LoopbackPort = 50100;
        public const string LoopbackUrl = "http://127.0.0.1";

        public const string ContentTypeHeader = "Content-Type";
        public const string DefaultContentType = "application/json";

        public static readonly string[] AuthScopes =
        {
            "openid",
            "offline_access",
            "profile",
            "email"
        };

        /// <summary>
        /// A string value which is used to encrypt PlayerPrefs data
        /// </summary>
        public const string EncryptionKey = "colizeum";

        /// <summary>
        /// A PlayerPrefs key under which the encrypted Access Token will be stored
        /// </summary>
        public const string AccessTokenPref = "ColizeumAccessToken";

        /// <summary>
        /// A PlayerPrefs key under which the encrypted Refresh Token will be stored
        /// </summary>
        public const string RefreshTokenPref = "ColizeumRefreshToken";

        /// <summary>
        /// A PlayerPrefs key under which the encrypted ID Token will be stored
        /// </summary>
        public const string IdTokenPref = "ColizemIdToken";
    }
}