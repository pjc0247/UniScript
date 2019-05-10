using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ModPlayerSDK
{
    using Internal;

    public class CreateAppWindow : EditorWindow
    {
        public static void ShowWindow()
        {
            var win = new CreateAppWindow();
            win.title = "CreateApp";
            win.maxSize = win.minSize = new Vector2(300, 70);
            var position = win.position;
            position.center = new Rect(0f, 0f, Screen.currentResolution.width, Screen.currentResolution.height).center;
            win.position = position;
            win.Show();
        }

        private string appName;
        private string thumbnailPath;
        private Texture2D thumbnail;

        private async void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(thumbnail, GUILayout.Width(50), GUILayout.Height(50)))
            {
                thumbnailPath = EditorUtility.OpenFilePanelWithFilters(
                    "Select a thumbnail",
                    "", new string[] { "*.png;*.jpg", "png,jpg" });

                var www = new WWW("file://" + thumbnailPath);
                while (www.isDone == false) ;

                thumbnail = www.texture;
            }

            appName = EditorGUILayout.TextField("AppName", appName);
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Create"))
            {
                if (string.IsNullOrEmpty(appName))
                    return;

                EditorUtility.DisplayProgressBar("ModPlayerSDK", "Hold on", 0);
                var resp = await App.CreateApp(appName);

                if (thumbnailPath != null)
                {
                    var fileRef = ModPlayerFB.Storage
                        .GetReference($"thumbnails/{resp.app.id}");

                    EditorUtility.DisplayProgressBar("ModPlayerSDK", "Hold on", 0.5f);
                    await fileRef.PutFileAsync(thumbnailPath);
                    var url = await fileRef.GetDownloadUrlAsync();

                    EditorUtility.DisplayProgressBar("ModPlayerSDK", "Hold on", 0.9f);
                    await App.SetThumbnail(resp.app, url.ToString());
                }

                EditorUtility.ClearProgressBar();

                Close();
            }
        }
    }
}