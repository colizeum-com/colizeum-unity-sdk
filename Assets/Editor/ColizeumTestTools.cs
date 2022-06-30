using System;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class ColizeumTestTools : EditorWindow
    {
        [MenuItem("Colizeum/Test Tools")]
        private static void ShowWindow()
        {
            var window = GetWindow<ColizeumTestTools>();
            window.titleContent = new GUIContent("Colizeum Test Tools");
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Space(20);

            if (GUILayout.Button("Clear PlayerPrefs"))
            {
                PlayerPrefs.DeleteAll();
            }
        }
    }
}