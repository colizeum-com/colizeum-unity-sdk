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
using ColizeumSDK.Enum;
using ColizeumSDK.Settings;
using UnityEditor;
using UnityEngine;

namespace ColizeumSDK.Editor
{
    internal class ColizeumSettingsEditor : EditorWindow
    {
        private static GUIStyle AreaStyle => GUI.skin.box;
        private static GUIStyle HeaderStyle => EditorStyles.boldLabel;

        private int _authPlatform = 0;

        private readonly string[] _authPlatforms = { "Default", "Standalone", "WebGL", "iOS", "Android" };

        [MenuItem("Colizeum/Edit Settings")]
        private static void ShowWindow()
        {
            var window = GetWindow<ColizeumSettingsEditor>();
            window.titleContent = new GUIContent("Colizeum Settings");
            window.Show();
        }

        private void OnGUI()
        {
            GeneralContainer();
            LoginContainer();
        }

        private void GeneralContainer()
        {
            var instance = ColizeumSettings.Instance;

            GUILayout.Space(20);
            GUILayout.BeginVertical(AreaStyle);
            EditorGUILayout.LabelField("General", HeaderStyle);
            GUILayout.Space(10);

            instance.appId = GetField("App ID", "", ref instance.appId);
            instance.apiMode = (ApiMode)EditorGUILayout.EnumPopup("Mode", instance.apiMode);
            instance.logLevel = (LogLevel)EditorGUILayout.EnumPopup("Log Level", instance.logLevel);

            EditorGUILayout.EndVertical();
        }

        private void LoginContainer()
        {
            var instance = ColizeumSettings.Instance;

            GUILayout.Space(20);
            GUILayout.BeginVertical(AreaStyle);
            EditorGUILayout.LabelField("Auth", HeaderStyle);
            GUILayout.Space(10);

            _authPlatform = GUILayout.Toolbar(_authPlatform, _authPlatforms);

            GUILayout.Space(10);

            switch (_authPlatform)
            {
                case 0:
                    AuthBlock(ref instance.defaultAuth, true);
                    break;
                case 1:
                    AuthBlock(ref instance.standaloneAuth);
                    break;
                case 2:
                    AuthBlock(ref instance.webGlAuth, false, true);
                    break;
                case 3:
                    AuthBlock(ref instance.iosAuth);
                    break;
                case 4:
                    AuthBlock(ref instance.androidAuth);
                    break;
            }

            EditorGUILayout.EndVertical();
        }

        private static void AuthBlock(ref AuthSettings settings, bool isDefault = false, bool isWebgl = false)
        {
            if (!isDefault)
            {
                GUILayout.Space(10);

                settings.enabled = GUILayout.Toggle(settings.enabled, "Override default values?");

                GUILayout.Space(10);
            }

            EditorGUI.BeginDisabledGroup(!settings.enabled && !isDefault);

            settings.clientId = GetField("OAuth Client ID", "", ref settings.clientId);

            if (!isWebgl)
            {
                settings.callbackType = (CallbackType)EditorGUILayout.EnumPopup("Callback Type", settings.callbackType);

                switch (settings.callbackType)
                {
                    case CallbackType.DeepLink:
                        GUILayout.Label("https://docs.unity3d.com/Manual/deep-linking.html");
                        GUILayout.Space(10);

                        settings.urlScheme = GetField("URL Scheme",
                            "Custom URI scheme you defined when setting up Deep linking", ref settings.urlScheme);
                        break;
                    case CallbackType.Redirect:
                        settings.redirectUri = GetField("Redirect URI",
                            "Redirect URI where the OAuth flow will redirect the user after the login",
                            ref settings.redirectUri);
                        break;
                    case CallbackType.LoopBack:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (isDefault && ColizeumSettings.Instance.Platform == PlatformType.WebGL &&
                    settings.callbackType != CallbackType.Redirect)
                {
                    GUILayout.Space(5);
                    GUILayout.Label("WebGL builds only support Redirect based callbacks", new GUIStyle
                        {
                            normal = new GUIStyleState
                            {
                                textColor = Color.red
                            },
                            fontStyle = FontStyle.Bold,
                        }
                    );
                }
            }
            else
            {
                settings.redirectUri = GetField("Redirect URI",
                    "Redirect URI where the OAuth flow will redirect the user after the login",
                    ref settings.redirectUri);
            }

            GUILayout.Space(10);
            EditorGUI.EndDisabledGroup();
        }

        private static string GetField(string label, string tooltip, ref string value)
        {
            return EditorGUILayout.TextField(new GUIContent(label, tooltip), value);
        }
    }
}