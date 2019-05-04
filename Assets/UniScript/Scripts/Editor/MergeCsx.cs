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

    public static void CreateMonolith(string path, string assetBundle)
    {
        CreateMonolith(path);
        AssetImporter.GetAtPath(path).SetAssetBundleNameAndVariant(assetBundle, "");
    }

    [MenuItem("UniScript/Manual/CreateMonolith")]
    public static void CreateMonolith(string path = "Assets/UniScript/Resources/uniscript/monolith.txt")
    {
        Debug.Log("<b>[UniScript]</b> CreateMonolith");

        var monolith = new Dictionary<string, string>();

        foreach (var csx in AssetDatabase.GetAllAssetPaths()
            .Where(x => x.EndsWith(".csx")))
        {
            monolith[csx] = File.ReadAllText(csx);
        }

        var json = UniScriptInternal.MiniJSON.Json.Serialize(monolith);
        var outPath = path;
        File.WriteAllText(outPath, json);

        AssetDatabase.ImportAsset(outPath);

        Debug.Log($"<b>[UniScript]</b> Merged {monolith.Count} scripts.");
        Debug.Log(json);
    }
}