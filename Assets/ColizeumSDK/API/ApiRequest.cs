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
using UnityEngine;

namespace ColizeumSDK.API
{
    /// <summary>
    /// A helper class which is used to create a new API request 
    /// </summary>
    public class ApiRequest
    {
        /// <summary>
        /// API Uri to which the request will be sent
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// Defines the HTTP method, either GET, HEAD, POST, PUT, DELETE
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Data to send to the API using generic object which will be serialized
        /// </summary>
        public object Body { get; set; }

        /// <summary>
        /// Data to send to the API using a WWWForm
        /// </summary>
        public WWWForm FormData { get; set; }

        /// <summary>
        /// Data to send to the API using a Dictionary
        /// </summary>
        public Dictionary<string, string> SimpleForm { get; set; }


        private Dictionary<string, string> _headers;

        /// <summary>
        /// The HTTP headers added manually to send with the request
        /// </summary>
        public Dictionary<string, string> Headers
        {
            get => _headers ??= new Dictionary<string, string>();
            set => _headers = value;
        }

        private Dictionary<string, string> _params;

        /// <summary>
        /// The HTTP query string params to send with the request
        /// </summary>
        public Dictionary<string, string> Params
        {
            get => _params ??= new Dictionary<string, string>();
            set => _params = value;
        }

        /// <summary>
        /// Defines if the API request should automatically attach the "Authorization" header
        /// </summary>
        public bool UseAuthorization = true;
    }
}