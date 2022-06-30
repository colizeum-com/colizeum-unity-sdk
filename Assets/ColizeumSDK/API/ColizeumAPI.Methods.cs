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
using UnityEngine.Networking;

namespace ColizeumSDK.API
{
    using Responses;

    public partial class ColizeumAPI
    {
        private Coroutine Get(ApiRequest request, Action onSuccess = null, Action<Exception> onError = null)
        {
            return Get<GenericResponse>(request, (_) => onSuccess?.Invoke(), onError);
        }

        private Coroutine Get<T>(ApiRequest request, Action<T> onSuccess = null, Action<Exception> onError = null)
        {
            request.Method = UnityWebRequest.kHttpVerbGET;

            return Request<T>(request, onSuccess, onError);
        }

        private Coroutine Head(ApiRequest request, Action onSuccess = null, Action<Exception> onError = null)
        {
            return Head<GenericResponse>(request, (_) => onSuccess?.Invoke(), onError);
        }

        private Coroutine Head<T>(ApiRequest request, Action<T> onSuccess = null, Action<Exception> onError = null)
        {
            request.Method = UnityWebRequest.kHttpVerbHEAD;

            return Request<T>(request, onSuccess, onError);
        }

        private Coroutine Post(ApiRequest request, Action onSuccess = null, Action<Exception> onError = null)
        {
            return Post<GenericResponse>(request, (_) => onSuccess?.Invoke(), onError);
        }

        private Coroutine Post<T>(ApiRequest request, Action<T> onSuccess = null, Action<Exception> onError = null)
        {
            request.Method = UnityWebRequest.kHttpVerbPOST;

            return Request<T>(request, onSuccess, onError);
        }

        private Coroutine Put(ApiRequest request, Action onSuccess = null, Action<Exception> onError = null)
        {
            return Put<GenericResponse>(request, (_) => onSuccess?.Invoke(), onError);
        }

        private Coroutine Put<T>(ApiRequest request, Action<T> onSuccess = null, Action<Exception> onError = null)
        {
            request.Method = UnityWebRequest.kHttpVerbPUT;

            return Request<T>(request, onSuccess, onError);
        }

        private Coroutine Delete(ApiRequest request, Action onSuccess = null, Action<Exception> onError = null)
        {
            return Delete<GenericResponse>(request, (_) => onSuccess?.Invoke(), onError);
        }

        private Coroutine Delete<T>(ApiRequest request, Action<T> onSuccess = null, Action<Exception> onError = null)
        {
            request.Method = UnityWebRequest.kHttpVerbDELETE;

            return Request<T>(request, onSuccess, onError);
        }

        private Coroutine Request(ApiRequest request, Action onSuccess = null, Action<Exception> onError = null)
        {
            return Request<GenericResponse>(request, _ => { onSuccess?.Invoke(); }, onError);
        }

        private Coroutine Request<T>(ApiRequest request, Action<T> onSuccess = null, Action<Exception> onError = null)
        {
            return StartCoroutine(Perform(request, onSuccess, onError));
        }
    }
}