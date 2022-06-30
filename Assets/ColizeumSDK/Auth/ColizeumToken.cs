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

namespace ColizeumSDK.Auth
{
    using Utils;

    /// <summary>
    /// A class which holds currently active tokens and manages token encryption in PlayerPrefs
    /// </summary>
    public class ColizeumToken : MonoBehaviour
    {
        /// <summary>
        /// An event which is called when the SDK fails to get a new access token, which usually means that the refresh token has expired
        /// </summary>
        public Action OnInvalid;
        
        /// <summary>
        /// Access Token which is used to access data on behalf of the Colizeum user
        /// </summary>
        public string AccessToken { get; private set; }

        /// <summary>
        /// Refresh Token which is used to get a new Access Token
        /// </summary>
        public string RefreshToken { get; private set; }

        /// <summary>
        /// ID Token that proves that the Colizeum user has been authenticated
        /// </summary>
        public string IDToken { get; private set; }

        private void Awake()
        {
            SetValues();
        }

        /// <summary>
        /// Returns a boolean indicating if there exists an active token instance
        /// </summary>
        /// <returns>Boolean</returns>
        public bool Exists()
        {
            return AccessToken != null;
        }

        /// <summary>
        /// Creates a new token instance and stores encrypted data
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="refreshToken"></param>
        /// <param name="idToken"></param>
        /// <returns></returns>
        public void Create(string accessToken, string refreshToken, string idToken)
        {
            PlayerPrefs.SetString(Constants.AccessTokenPref,
                !string.IsNullOrEmpty(refreshToken) ? Cryptography.Encrypt(accessToken) : "");
            PlayerPrefs.SetString(Constants.RefreshTokenPref,
                !string.IsNullOrEmpty(refreshToken) ? Cryptography.Encrypt(refreshToken) : "");
            PlayerPrefs.SetString(Constants.IdTokenPref,
                !string.IsNullOrEmpty(refreshToken) ? Cryptography.Encrypt(idToken) : "");

            SetValues();
        }

        /// <summary>
        /// Destroys the current token instance and also clears the store data
        /// </summary>
        public void Destroy()
        {
            PlayerPrefs.DeleteKey(Constants.AccessTokenPref);
            PlayerPrefs.DeleteKey(Constants.RefreshTokenPref);
            PlayerPrefs.DeleteKey(Constants.IdTokenPref);

            SetValues();
        }

        /// <summary>
        /// Tries to refresh the current access token using the stored refresh token
        /// </summary>
        /// <param name="onSuccess"></param>
        /// <param name="onError"></param>
        public void Refresh(Action onSuccess, Action<Exception> onError)
        {
            AccessToken = null;

            if (RefreshToken == null)
            {
                onError.Invoke(new NullReferenceException("Missing refresh token"));

                return;
            }

            Colizeum.API.RefreshAccessToken(RefreshToken, (response) =>
            {
                Create(response.access_token, response.refresh_token, response.id_token);

                onSuccess.Invoke();
            }, onError);
        }

        private void SetValues()
        {
            var accessToken = PlayerPrefs.GetString(Constants.AccessTokenPref, null);
            var refreshToken = PlayerPrefs.GetString(Constants.RefreshTokenPref, null);
            var idToken = PlayerPrefs.GetString(Constants.IdTokenPref, null);

            AccessToken = !string.IsNullOrEmpty(accessToken) ? Cryptography.Decrypt(accessToken) : null;
            RefreshToken = !string.IsNullOrEmpty(refreshToken) ? Cryptography.Decrypt(refreshToken) : null;
            IDToken = !string.IsNullOrEmpty(idToken) ? Cryptography.Decrypt(idToken) : null;
        }
    }
}