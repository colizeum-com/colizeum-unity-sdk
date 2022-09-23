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
using System.Security.Cryptography;
using System.Text;
using ColizeumSDK.Factories;
using UnityEngine;
using UnityEngine.Networking;
using Random = System.Random;

namespace ColizeumSDK.Auth
{
    using API.Responses;
    using Callbacks;
    using Enum;
    using Exceptions;
    using Models;
    using Settings;
    using Utils;

    /// <summary>
    /// Colizeum Authorization module which allows to integrate "Login with Colizeum" functionality easily
    /// </summary>
    public class ColizeumAuth : MonoBehaviour
    {
        private string _state;
        private string _codeVerifier;

        private Action<User> _onSuccess;
        private Action<Exception> _onError;

        private void OnDestroy()
        {
            if (ColizeumSettings.Instance.CallbackType == CallbackType.LoopBack)
            {
                Loopback.Stop();
            }
        }

        /// <summary>
        /// Returns a boolean indicating if the user is logged in
        /// </summary>
        public bool IsLoggedIn => Colizeum.Token.IsValid() || Colizeum.Token.CanBeRefreshed();

        /// <summary>
        /// Returns the authorization url with the code challenge and state
        /// </summary>
        /// <returns>URL</returns>
        public string GetAuthUrl()
        {
            var settings = ColizeumSettings.Instance;

            _codeVerifier ??= GenerateCodeVerifier();
            _state ??= GenerateState(16);

            return Constants.Issuer + "/auth?response_type=code" +
                   "&client_id=" + settings.ClientId +
                   "&redirect_uri=" + UnityWebRequest.EscapeURL(settings.RedirectUri) +
                   "&scope=" + UnityWebRequest.EscapeURL(string.Join(" ", Constants.AuthScopes)) +
                   "&state=" + _state +
                   "&code_challenge=" + GenerateCodeChallenge(_codeVerifier) +
                   "&code_challenge_method=S256" +
                   "&prompt=consent";
        }

        /// <summary>
        /// Opens the system's internet browser and starts the login flow 
        /// </summary>
        /// <param name="onSuccess">Called after a successful login</param>
        /// <param name="onError">Called when an exception is thrown within the login flow</param>
        public void Login(Action<User> onSuccess, Action<Exception> onError = null)
        {
            _onSuccess = onSuccess;
            _onError = onError;

            if (IsLoggedIn)
            {
                GetUser(onSuccess, onError);

                return;
            }

            var settings = ColizeumSettings.Instance;

            switch (settings.CallbackType)
            {
                case CallbackType.DeepLink:
                    WaitForDeepLink();
                    break;
                case CallbackType.LoopBack:
                    WaitForLoopback();
                    break;
                case CallbackType.Redirect:
                    OpenBrowser();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Destroys the active Colizeum session, removes all in-memory user data and calls the API to revoke access tokens
        /// </summary>
        public void Logout()
        {
            if (Colizeum.Token.RefreshToken == null) return;

            Colizeum.API.RevokeToken(Colizeum.Token.RefreshToken);

            Colizeum.Token.Destroy();

            Colizeum.User = null;
        }

        /// <summary>
        /// Returns the currently logged in user
        /// </summary>
        /// <param name="onSuccess"></param>
        /// <param name="onError"></param>
        public void GetUser(Action<User> onSuccess, Action<Exception> onError = null)
        {
            Colizeum.API.GetMe(userResponse =>
            {
                Colizeum.User = UserFactory.Create(userResponse.item);

                onSuccess.Invoke(Colizeum.User);
            }, onError);
        }

        public void GetTokensFromCode(string code, Action<TokenResponse> onSuccess, Action<Exception> onError = null)
        {
            
        }

        private void WaitForDeepLink()
        {
            var isListening = DeepLink.Listen(OnCodeReceived, OnError);

            if (!isListening) return;

            OpenBrowser();
        }

        private void WaitForLoopback()
        {
            Loopback.Listen();

            StartCoroutine(Loopback.WaitForResponse(OnCodeReceived, OnError));

            OpenBrowser();
        }

        private void OnCodeReceived(CodeResponse response)
        {
            if (response.state != _state)
            {
                throw new AuthCodeException("Incoming state didn't match the stored one");
            }

            Colizeum.API.GetTokensFromCode(response.code, _codeVerifier, OnTokensReceived, OnError);
        }

        private void OnTokensReceived(TokenResponse response)
        {
            Colizeum.Token.Create(response.access_token, response.refresh_token, response.id_token, response.expires_in);

            GetUser(user => { _onSuccess.Invoke(user); });
        }

        private void OnError(Exception exception)
        {
            _onError?.Invoke(exception);

            CleanUp();
        }

        private void CleanUp()
        {
            _codeVerifier = null;
            _state = null;

            _onSuccess = null;
            _onError = null;
        }

        private void OpenBrowser()
        {
            Application.OpenURL(GetAuthUrl());
        }

        private static string GenerateCodeVerifier()
        {
            var rng = RandomNumberGenerator.Create();

            var bytes = new byte[32];
            rng.GetBytes(bytes);

            // It is recommended to use a URL-safe string as code_verifier.
            // See section 4 of RFC 7636 for more details.
            return Convert.ToBase64String(bytes)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }

        private static string GenerateCodeChallenge(string verifier)
        {
            using var sha256 = SHA256.Create();

            var challengeBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(verifier));

            return Convert.ToBase64String(challengeBytes)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }

        private static string GenerateState(int length)
        {
            var random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}