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

namespace ColizeumSDK.Settings
{
    using Enum;
    using Utils;

    [Serializable]
    public class AuthSettings
    {
        public bool enabled;
        public string clientId;
        public CallbackType callbackType = CallbackType.DeepLink;
        public string redirectUri;
        public string urlScheme;

        public string ClientId => clientId;

        public string RedirectUri
        {
            get
            {
#if UNITY_EDITOR
                return $"{Constants.LoopbackUrl}:{Constants.LoopbackPort}";
#elif UNITY_WEBGL
                return redirectUri;
#else
                return callbackType switch
                {
                    CallbackType.DeepLink => $"{urlScheme}://colizeum-auth",
                    CallbackType.Redirect => redirectUri,
                    CallbackType.LoopBack => $"{Constants.LoopbackUrl}:{Constants.LoopbackPort}",
                    _ => throw new ArgumentOutOfRangeException()
                };
#endif
            }
        }
    }
}