using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ModPlayerSDK {
    public class AppDashboardWindow : EditorWindow
    {
        [MenuItem("ModPlayer/AppDashboard")]
        public static void ShowDashboard()
        {
            var win = new AppDashboardWindow();
            win.Show();
        }

        private ModApp[] apps;

        private Vector2 appsScroll;

        async void Awake()
        {
            apps = (await App.GetMyApps()).apps;
        }

        void OnGUI()
        {
            GUI.DrawTexture(new Rect(Vector2.zero, maxSize), Texture2D.whiteTexture);

            EditorGUILayout.BeginVertical();

            if (GUILayout.Button("Add"))
            {
                CreateApp("A", "B");
            }

            appsScroll = EditorGUILayout.BeginScrollView(appsScroll);
            if (apps != null)
            {
                foreach (var app in apps)
                {
                    EditorGUILayout.LabelField(app.title);
                    EditorGUILayout.LabelField(app.description);
                }
            }
            EditorGUILayout.EndScrollView();

            EditorGUILayout.EndVertical();
        }

        private async void CreateApp(string title, string description)
        {
            EditorUtility.DisplayProgressBar("ModPlayerSDK", "Create a new app...", 0);
            await App.CreateApp(title, description);
            EditorUtility.ClearProgressBar();
        }
    }
}