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
using System.Collections;
using System.Net;
using System.Text;
using UnityEngine;

namespace ColizeumSDK.Auth.Callbacks
{
    using API.Responses;
    using Enum;
    using Exceptions;
    using Settings;
    using Utils;

    internal static class Loopback
    {
        public static bool Processed;
        public static CodeResponse CodeResponse;
        public static Exception Error;

        private static HttpListener _listener;

        /// <summary>
        /// Starts the HTTP listener and waits for incoming code request
        /// </summary>
        public static void Listen()
        {
            if (ColizeumSettings.Instance.Platform == PlatformType.WebGL)
            {
                throw new PlatformNotSupportedException("WebGL doesn't support loopback callback");
            }

            Processed = false;

            // We only want to allow one instance of the HTTP listener
            Stop();

            _listener = new HttpListener();

            _listener.Prefixes.Add($"{Constants.LoopbackUrl}:{Constants.LoopbackPort}/");
            _listener.Start();
            _listener.BeginGetContext(IncomingHttpRequest, _listener);

            Debug.Log("Started the Loopback listener on port " + Constants.LoopbackPort);
        }

        public static void Stop()
        {
            if (_listener == null) return;

            _listener.Stop();
            _listener.Prefixes.Clear();
            _listener = null;

            Debug.Log("Stopped the Loopback listener");
        }

        private static void IncomingHttpRequest(IAsyncResult result)
        {
            var httpListener = (HttpListener)result.AsyncState;
            var httpContext = httpListener.EndGetContext(result);
            var httpRequest = httpContext.Request;

            var code = httpRequest.QueryString.Get("code");
            var state = httpRequest.QueryString.Get("state");

            try
            {
                if (!string.IsNullOrEmpty(code))
                {
                    CodeResponse = new CodeResponse
                    {
                        code = code,
                        state = state
                    };
                }
                else
                {
                    var message = httpRequest.QueryString.Get("error");

                    throw new AuthCodeException(message);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);

                Error = e;
            }
            finally
            {
                var httpResponse = httpContext.Response;

                var response =
                    "<html><body><div style='text-align:center'><b>DONE!</b><br>You can now switch back to the game</div></body></html>";

                var buffer = Encoding.UTF8.GetBytes(response);

                httpResponse.ContentLength64 = buffer.Length;

                var output = httpResponse.OutputStream;

                output.Write(buffer, 0, buffer.Length);
                output.Close();

                // the HTTP listener has served it's purpose, shut it down
                Stop();

                Processed = true;
            }
        }

        public static IEnumerator WaitForResponse(Action<CodeResponse> onSuccess, Action<Exception> onError = null)
        {
            yield return new WaitUntil(() => Processed);

            if (CodeResponse != null)
            {
                onSuccess.Invoke(CodeResponse);
            }

            if (Error != null)
            {
                onError?.Invoke(Error);
            }
        }
    }
}