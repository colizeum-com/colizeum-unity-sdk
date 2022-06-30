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

using UnityEngine;

namespace ColizeumSDK
{
    using API;
    using Auth;
    using Models;
    using Utils;

    /// <summary>
    /// The central point of the SDK which holds references to all the necessary modules
    /// </summary>
    public class Colizeum : MonoBehaviour
    {
        private static Colizeum _instance;

        private ColizeumAuth _auth;
        private ColizeumAPI _api;
        private ColizeumToken _token;

        private static Colizeum Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Create();
                }

                return _instance;
            }
        }

        /// <summary>
        /// Reference the the ColizeumAuth module
        /// </summary>
        /// <seealso cref="ColizeumAuth"/>
        public static ColizeumAuth Auth => Instance._auth;

        /// <summary>
        /// Reference the the ColizeumApi module
        /// </summary>
        /// <seealso cref="ColizeumAPI"/>
        public static ColizeumAPI API => Instance._api;

        /// <summary>
        /// Reference to the ColizeumToken class
        /// </summary>
        /// <see cref="ColizeumToken"/>
        public static ColizeumToken Token => Instance._token;


        /// <summary>
        /// Reference to the currently logged in user
        /// </summary>
        /// <see cref="User"/>
        public static User User { get; set; }

        /// <summary>
        /// A boolean which represents if the SDK is initialized or not
        /// </summary>
        public static bool IsInitialized => Instance != null;

        /// <summary>
        /// Returns a boolean indicating if the user is logged in
        /// </summary>
        public static bool IsLoggedIn => Auth != null && Auth.IsLoggedIn;

        private static Colizeum Create()
        {
            var gameObject = new GameObject("Colizeum");

            DontDestroyOnLoad(gameObject);

            _instance = gameObject.AddComponent<Colizeum>();

            _instance._auth = gameObject.AddComponent<ColizeumAuth>();
            _instance._api = gameObject.AddComponent<ColizeumAPI>();
            _instance._token = gameObject.AddComponent<ColizeumToken>();

#if UNITY_EDITOR
            Debug.Log("Running in editor. Some actions will be emulated");
#endif

            return _instance;
        }
    }
}