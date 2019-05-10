using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ModPlayerSDK
{
    using Model;

    public class AppDashboardWindow : ModPlayerWindow
    {
        [MenuItem("ModPlayer/AppDashboard")]
        public static void ShowDashboard()
        {
            var win = new AppDashboardWindow();
            win.maxSize = win.minSize = new Vector2(350, 400);
            var position = win.position;
            position.center = new Rect(0f, 0f, Screen.currentResolution.width, Screen.currentResolution.height).center;
            win.position = position;
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

        protected override void Draw()
        {
            if (GUILayout.Button("Add"))
                CreateAppWindow.ShowWindow();

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
        }

        private async void CreateApp(string title)
        {
            EditorUtility.DisplayProgressBar("ModPlayerSDK", "Create a new app...", 0);
            await App.CreateApp(title);
            EditorUtility.ClearProgressBar();
        }
    }
}