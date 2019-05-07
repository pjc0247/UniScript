using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ModPlayerSDK
{
    using Model;

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
            //apps = (await App.GetMyApps()).apps;

            apps = new ModApp[] {
                new ModApp(){name = "ASDF"},
                new ModApp(){name = "ASDF"},
                new ModApp(){name = "qwer"},
                new ModApp(){name = "ASDF"},
            };
        }

        void OnGUI()
        {
            GUI.DrawTexture(new Rect(Vector2.zero, maxSize), Texture2D.whiteTexture);

            EditorGUILayout.BeginVertical();

            if (GUILayout.Button("Add"))
            {
                CreateApp("A");
            }

            appsScroll = EditorGUILayout.BeginScrollView(appsScroll);
            if (apps != null)
            {
                foreach (var app in apps)
                {
                    EditorGUILayout.BeginHorizontal(GUI.skin.box);

                    if (app.thumbnail == null)
                        GUILayout.Box(Texture2D.whiteTexture, GUILayout.Width(50), GUILayout.Height(50));
                    else
                        GUILayout.Box(app.thumbnail, GUILayout.Width(50), GUILayout.Height(50));

                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.LabelField(app.name);
                    GUILayout.Space(5);
                    EditorGUILayout.LabelField(app.name);
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndScrollView();

            EditorGUILayout.EndVertical();
        }

        private async void CreateApp(string title)
        {
            EditorUtility.DisplayProgressBar("ModPlayerSDK", "Create a new app...", 0);
            await App.CreateApp(title);
            EditorUtility.ClearProgressBar();
        }
    }
}