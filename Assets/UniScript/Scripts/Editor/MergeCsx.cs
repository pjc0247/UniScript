using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

class MergeCsx : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        CreateMonolith();
    }

    [MenuItem("UniScript/Manual/CreateMonolith")]
    public static void CreateMonolith()
    {
        var monolith = new Dictionary<string, string>();

        foreach (var path in AssetDatabase.GetAllAssetPaths()
            .Where(x => x.EndsWith(".csx")))
        {
            monolith[path] = File.ReadAllText(path);
        }

        var json = UniScriptInternal.MiniJSON.Json.Serialize(monolith);
        var outPath = "Assets/UniScript/Resources/uniscript/monolith.txt";
        File.WriteAllText(outPath, json);

        AssetDatabase.ImportAsset(outPath);
    }
}