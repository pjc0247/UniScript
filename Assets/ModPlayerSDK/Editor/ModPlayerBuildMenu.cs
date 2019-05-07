using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace ModPlayerSDK
{
    using Internal;
    using Model;

    public class ModPlayerBuildMenu : EditorWindow
    {
        [MenuItem("ModPlayer/Publish")]
        public static void ShowPublishWindow()
        {
            var win = new ModPlayerBuildMenu();
            win.minSize = win.maxSize = new Vector2(450, 300);
            win.titleContent = new GUIContent(
                "Publish",
                AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/ModPlayerSDK/Images/icon.png"));
            win.Show();
        }

        private Texture2D logo;

        private string modName;
        private string version;
        private string description;
        private int appIdx;

        private ModApp targetApp;
        private ModApp[] apps;
        private string[] appTitles;

        async void Awake()
        {
            apps = (await App.GetMyApps()).apps;
            appTitles = apps.Select(x => x.name).ToArray();
        }

        public void OnGUI()
        {
            EnsureResources();

            GUI.DrawTexture(new Rect(0, 0, 450, 400), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(0, 0, 450, 100), logo);

            EditorGUILayout.BeginVertical();
            GUILayout.Space(110);

            if (appTitles != null)
                appIdx = EditorGUILayout.Popup("App", appIdx, appTitles);
            else
                EditorGUILayout.HelpBox("Please wait for fetching app list", MessageType.Info);
                
            modName = EditorGUILayout.TextField("Mod name", modName);
            version = EditorGUILayout.TextField("Version", version);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Description", GUILayout.Width(146));
            description = EditorGUILayout.TextArea(description,
                GUILayout.Height(40));
            EditorGUILayout.EndHorizontal();

            if (GUI.Button(new Rect(340, 270, 100, 30), "Publish"))
            {
                if (string.IsNullOrEmpty(modName) == false)
                {
                    targetApp = apps[appIdx];
                    BuildMod();
                }
            }

            EditorGUILayout.EndVertical();
        }

        private void EnsureResources()
        {
            if (logo == null)
                logo = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/ModPlayerSDK/Images/modplayer.png");
        }

        private async void BuildMod()
        {
            var scene = EditorSceneManager.GetActiveScene();

            EditorUtility.DisplayProgressBar("ModPlayerSDK", "Building...", 0);
            UniMigration.MigrateScene();

            AssetImporter.GetAtPath(scene.path)
                .SetAssetBundleNameAndVariant($"{modName}_scene", "");

            MergeCsx.CreateMonolith(
                "Assets/modplayer_script.json",
                $"{modName}_script");

            UniAssetBundle.BuildUniScriptScene(
                $"{modName}_scene", $"{modName}_script");

            UniMigration.EndMigration();

            EditorUtility.DisplayProgressBar("ModPlayerSDK", "Uploading...", 0);
            await UploadMod();
            EditorUtility.ClearProgressBar();
        }
        private async Task UploadMod()
        {
            var storage = ModPlayerFB.Storage;

            var scene = storage.GetReference($"mod/{modName}/scene");
            var script = storage.GetReference($"mod/{modName}/script");

            try
            {
                await scene.PutFileAsync($"Packs/{modName}_scene");
                EditorUtility.DisplayProgressBar("ModPlayerSDK", "Uploading...", 33);
                await script.PutFileAsync($"Packs/{modName}_script");
                EditorUtility.DisplayProgressBar("ModPlayerSDK", "Uploading...", 66);

                var sceneUrl = (await scene.GetDownloadUrlAsync()).ToString();
                var scriptUrl = (await script.GetDownloadUrlAsync()).ToString();
                await App.AddBuild(targetApp,
                    new ModBuild()
                    {
                        scene_url = sceneUrl,
                        script_url = scriptUrl
                    });

                EditorUtility.ClearProgressBar();

                EditorUtility.DisplayDialog("ModPlayerSDK", "Upload complete!", "Ok");

                Close();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}