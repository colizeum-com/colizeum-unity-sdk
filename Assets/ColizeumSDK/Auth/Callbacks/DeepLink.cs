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
using UnityEngine;

namespace ColizeumSDK.Auth.Callbacks
{
    using API.Responses;
    using Utils;

    internal static class DeepLink
    {
        private static Action<CodeResponse> _onSuccess;
        private static Action<Exception> _onError;

        public static bool Listen(Action<CodeResponse> onSuccess, Action<Exception> onError)
        {
            _onSuccess = onSuccess;
            _onError = onError;

            if (!string.IsNullOrEmpty(Application.absoluteURL))
            {
                OnActivated(Application.absoluteURL);

                return false;
            }

            Application.deepLinkActivated += OnActivated;

            return true;
        }

        private static void OnActivated(string address)
        {
            try
            {
                var response = Parser.ParseDeepLinkUrl(address);

                if (response == null)
                {
                    Debug.LogWarning("DeepLink URL was not meant for ColizeumSDK");

                    return;
                }

                _onSuccess.Invoke(response);
            }
            catch (Exception e)
            {
                _onError.Invoke(e);
            }

            _onSuccess = null;
            _onError = null;
        }
    }
}