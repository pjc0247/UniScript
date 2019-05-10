using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

using UniScript.Serialization;

namespace UniScript
{
    public class UniMigration : EditorWindow
    {
        private class ScriptData
        {
            public MonoBehaviour mono;
            public string srcPath;
            public string csxPath;
        }

        private static MigrationContext ctx;

        public static void MigrateAll()
        {
            ctx = new MigrationContext();

            var paths = new List<ScriptData>();

            CollectObjectsFromScene();
            CollectObjectsFromResources();

            var scene = EditorSceneManager.GetActiveScene();
            var backupPath = scene.path + ".backup.unity";
            if (File.Exists(backupPath))
                File.Delete(backupPath);
            File.Copy(scene.path, backupPath);

            var tempPath = "Assets/UniScript/Temp/";
            if (Directory.Exists(tempPath))
                Directory.Delete(tempPath, true);
            Directory.CreateDirectory(tempPath);
            Scriptify(paths);
        }
        private static void CollectObjectsFromResources()
        {
            foreach (var path in AssetDatabase.GetAllAssetPaths()
                .Where(x => x.Contains("Resources/"))
                .Where(x => x.EndsWith(".prefab")))
            {
                Debug.Log($"<b>[Migrate]</b> {path}");

                var prefab = PrefabUtility.LoadPrefabContents(path);
                var scriptData = new List<ScriptData>();

                AddScriptDataFromGameObject(prefab, scriptData);
                Migrate(scriptData);

                PrefabUtility.SaveAsPrefabAsset(prefab, path);
                PrefabUtility.UnloadPrefabContents(prefab);
            }
        }
        private static void CollectObjectsFromScene()
        {
            var scripts = GameObject.FindObjectsOfType<MonoBehaviour>();
            var scriptData = new List<ScriptData>();

            foreach (var script in scripts)
                AddScriptDataFromMonoBehaviour(script, scriptData);

            Migrate(scriptData);
        }
        private static void AddScriptDataFromGameObject(GameObject gobj, List<ScriptData> scriptData)
        {
            foreach (var script in gobj.GetComponentsInChildren<MonoBehaviour>())
            {
                if (script.GetType() == typeof(UniScriptBehaviour))
                    continue;

                AddScriptDataFromMonoBehaviour(script, scriptData);
            }
        }
        private static void AddScriptDataFromMonoBehaviour(MonoBehaviour script, List<ScriptData> scriptData)
        {
            var mono = MonoScript.FromMonoBehaviour(script);
            var path = AssetDatabase.GetAssetPath(mono);

            if (path.Contains("ModScript/") == false)
                return;

            scriptData.Add(new ScriptData() {
                mono = script,
                srcPath = path,
                csxPath = Path.Combine("Assets/UniScript/Temp/", Path.GetFileName(path) + "x")
            });
        }
        private static void Migrate(List<ScriptData> scriptData)
        {
            Debug.Log($"Migrate {scriptData.Count}");
            foreach (var data in scriptData)
            {
                var uniScript = data.mono
                    .gameObject
                    .AddComponent<UniFileScriptBehaviour>();

                uniScript.overrideFields = new SerializableDictionarySO();

                var type = data.mono.GetType();
                foreach (var f in type.GetFields())
                {
                    uniScript.overrideFields[f.Name] =
                        f.GetValue(data.mono);
                }

                uniScript.scriptPath = data.csxPath;

                DestroyImmediate(data.mono);
            }
        }

        [MenuItem("UniScript/test scene")]
        public static void MigrateObject()
        {
            MigrateAll();
        }

        [MenuItem("UniScript/Migrate scene")]
        public static void MigrateScene()
        {
            MigrateAll();
        }
        /// <summary>
        /// Ends migration and revert all changes.
        /// </summary>
        public static void EndMigration()
        {
            var scenePath = ctx.scenePath;
            var backupPath = scenePath + ".backup.unity";
            if (File.Exists(scenePath))
                File.Delete(scenePath);
            File.Copy(backupPath, scenePath);

            AssetDatabase.ImportAsset(scenePath, ImportAssetOptions.ForceUpdate);

            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            //EditorSceneManager.load

            ctx = null;
        }

        /// <summary>
        /// Converts given C# scripts(.cs) to UniScript files(.csx)
        /// </summary>
        private static void Scriptify(List<ScriptData> srcPath)
        {
            foreach (var path in srcPath)
            {
                var src = File.ReadAllText(path.srcPath);
                File.WriteAllText(path.csxPath, src);

                AssetDatabase.ImportAsset(path.csxPath);
            }
        }
    }
}