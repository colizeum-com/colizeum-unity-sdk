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

namespace ColizeumSDK.Utils
{
    using Enum;
    using Settings;
    
    internal static class Debug
    {
        public static void Log(object message)
        {
            if (ColizeumSettings.Instance.logLevel != LogLevel.Info) return;

            UnityEngine.Debug.Log("[Colizeum] " + message);
        }

        public static void LogWarning(object message)
        {
            if (ColizeumSettings.Instance.logLevel > LogLevel.Warning) return;

            UnityEngine.Debug.LogWarning("[Colizeum] " + message);
        }

        public static void LogError(object message)
        {
            UnityEngine.Debug.LogError("[Colizeum] " + message);
        }

        public static void LogAssertion(object message)
        {
            UnityEngine.Debug.LogAssertion("[Colizeum] " + message);
        }

        public static void LogException(Exception exception)
        {
            UnityEngine.Debug.LogException(exception);
        }
    }
}