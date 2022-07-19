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
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ColizeumSDK.Settings
{
    using Enum;

    [Serializable]
    public class ColizeumSettings : ScriptableObject
    {
        private const string AssetName = "ColizeumSettings";
        private const string AssetPath = "Resources/";
        private const string AssetExtension = ".asset";

        private static ColizeumSettings _instance;
        private static string _sdkPath;

        private void OnValidate()
        {
            MarkAssetDirty();
        }

        #region General

        // While currently its not used, left here it for the future
        public string appId = "123456";
        public ApiMode apiMode = ApiMode.Sandbox;
        public LogLevel logLevel = LogLevel.Warning;

#if UNITY_STANDALONE
        public PlatformType Platform => PlatformType.Standalone;
#elif UNITY_WEBGL
        public PlatformType Platform => PlatformType.WebGL;
#elif UNITY_ANDROID
        public PlatformType Platform => PlatformType.Android;
#elif UNITY_IOS
        public PlatformType Platform => PlatformType.IOS;
#endif

        #endregion

        #region Auth

        public AuthSettings defaultAuth;
        public AuthSettings webGlAuth;
        public AuthSettings androidAuth;
        public AuthSettings iosAuth;
        public AuthSettings standaloneAuth;

        public string ClientId
        {
            get
            {
#if UNITY_STANDALONE
                return _instance.standaloneAuth.enabled
                    ? _instance.standaloneAuth.ClientId
                    : _instance.defaultAuth.ClientId;
#elif UNITY_WEBGL
                return _instance.webGlAuth.enabled
                    ? _instance.webGlAuth.ClientId
                    : _instance.defaultAuth.ClientId;
#elif UNITY_IOS
                return _instance.iosAuth.enabled
                    ? _instance.iosAuth.ClientId
                    : _instance.defaultAuth.ClientId;
#elif UNITY_ANDROID
                return _instance.androidAuth.enabled
                    ? _instance.androidAuth.ClientId
                    : _instance.defaultAuth.ClientId;
#endif
            }
        }

        public string RedirectUri
        {
            get
            {
#if UNITY_STANDALONE
                return _instance.standaloneAuth.enabled
                    ? _instance.standaloneAuth.RedirectUri
                    : _instance.defaultAuth.RedirectUri;
#elif UNITY_WEBGL
                return _instance.webGlAuth.enabled
                    ? _instance.webGlAuth.RedirectUri
                    : _instance.defaultAuth.RedirectUri;
#elif UNITY_IOS
                return _instance.iosAuth.enabled
                    ? _instance.iosAuth.RedirectUri
                    : _instance.defaultAuth.RedirectUri;
#elif UNITY_ANDROID
                return _instance.androidAuth.enabled
                    ? _instance.androidAuth.RedirectUri
                    : _instance.defaultAuth.RedirectUri;
#endif
            }
        }

        public CallbackType CallbackType
        {
            get
            {
#if UNITY_EDITOR
                return CallbackType.LoopBack;
#elif UNITY_STANDALONE
                return _instance.standaloneAuth.enabled
                    ? _instance.standaloneAuth.callbackType
                    : _instance.defaultAuth.callbackType;
#elif UNITY_WEBGL
                return CallbackType.Redirect;
#elif UNITY_IOS
                return _instance.iosAuth.enabled
                    ? _instance.iosAuth.callbackType
                    : _instance.defaultAuth.callbackType;
#elif UNITY_ANDROID
                return _instance.androidAuth.enabled
                    ? _instance.androidAuth.callbackType
                    : _instance.defaultAuth.callbackType;
#endif
            }
        }

        #endregion

        public static ColizeumSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load(AssetName) as ColizeumSettings;
                    StoreSdkPath();
                }

                if (_instance != null)
                {
                    return _instance;
                }

                _instance = CreateInstance<ColizeumSettings>();

                SaveAsset(Path.Combine(GetSdkPath(), AssetPath), AssetName);

                return _instance;
            }
        }

        public static string GetSdkPath()
        {
            if (_sdkPath == null)
            {
                StoreSdkPath();
            }

            return _sdkPath;
        }

        private static void StoreSdkPath()
        {
            _sdkPath = GetAbsoluteSdkPath().Replace("\\", "/").Replace(Application.dataPath, "Assets");
        }

        private static string GetAbsoluteSdkPath()
        {
            return Path.GetDirectoryName(Path.GetDirectoryName(FindEditor(Application.dataPath)));
        }

        private static string FindEditor(string path)
        {
            foreach (var d in Directory.GetDirectories(path))
            {
                foreach (var f in Directory.GetFiles(d))
                {
                    if (f.Contains("SettingsEditor.cs"))
                    {
                        return f;
                    }
                }

                var rec = FindEditor(d);
                if (rec != null)
                {
                    return rec;
                }
            }

            return null;
        }

        private static void SaveAsset(string directory, string name)
        {
#if UNITY_EDITOR
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            AssetDatabase.CreateAsset(Instance, directory + name + AssetExtension);
            AssetDatabase.Refresh();
#endif
        }

        private static void MarkAssetDirty()
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(Instance);
#endif
        }
    }
}