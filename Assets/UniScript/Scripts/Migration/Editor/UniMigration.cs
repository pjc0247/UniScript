using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

using UniScript.Serialization;

public class UniMigration : EditorWindow
{
    private class ScriptData
    {
        public MonoBehaviour mono;
        public string srcPath;
        public string scriptPath;
    }

    private static string scenePath;

    [MenuItem("UniScript/Migrate scene")]
    public static void MigrateScene()
    {
        var scene = EditorSceneManager.GetActiveScene();
        var roots = scene.GetRootGameObjects();

        var scripts = GameObject.FindObjectsOfType<MonoBehaviour>();
        var paths = new List<ScriptData>();
        foreach (var script in scripts)
        {
            var mono = MonoScript.FromMonoBehaviour(script);
            var path = AssetDatabase.GetAssetPath(mono);

            if (path.Contains("ModScript/") == false)
                continue;

            paths.Add(new ScriptData() {
                mono = script,
                srcPath = path
            });
        }

        var tempPath = "Assets/UniScript/Temp/";
        if (Directory.Exists(tempPath))
            Directory.Delete(tempPath, true);
        Directory.CreateDirectory(tempPath);
        Scriptify(paths, tempPath);

        scenePath = scene.path;

        var backupPath = scene.path + ".backup.unity";
        if (File.Exists(backupPath))
            File.Delete(backupPath);
        File.Copy(scene.path, backupPath);

        foreach (var path in paths)
        {
            var uniScript = path.mono
                .gameObject
                .AddComponent<UniFileScriptBehaviour>();

            uniScript.overrideFields = new SerializableDictionarySO();

            var type = path.mono.GetType();
            foreach (var f in type.GetFields())
            {
                uniScript.overrideFields[f.Name] =
                    f.GetValue(path.mono);
            }

            uniScript.scriptPath = path.scriptPath;

            DestroyImmediate(path.mono);
        }
    }
    public static void EndMigration()
    {
        var backupPath = scenePath + ".backup.unity";
        if (File.Exists(scenePath))
            File.Delete(scenePath);
        File.Copy(backupPath, scenePath);

        AssetDatabase.ImportAsset(scenePath, ImportAssetOptions.ForceUpdate);

        EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        //EditorSceneManager.load
    }

    private static void Scriptify(List<ScriptData> srcPath, string dstPath)
    {
        foreach (var path in srcPath)
        {
            var src = File.ReadAllText(path.srcPath);
            var fname = Path.GetFileName(path.srcPath) + "x"; // .csx
            var finalPath = Path.Combine(dstPath, fname);
            File.WriteAllText(finalPath, src);

            path.scriptPath = finalPath;

            AssetDatabase.ImportAsset(finalPath);
        }
    }
}
