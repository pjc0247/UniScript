using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AutoReload : AssetPostprocessor
{
    private static Dictionary<string, List<UniFileScriptBehaviour>> scripts 
        = new Dictionary<string, List<UniFileScriptBehaviour>>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
#if UNITY_EDITOR
        var go = new GameObject("AutoloadIndicator");
        go.AddComponent<AutoLoadIndicator>();

        UniFileScriptBehaviour.registerScriptDelegate = (path, obj) => {
            if (scripts.ContainsKey(path) == false)
                scripts[path] = new List<UniFileScriptBehaviour>();
            scripts[path].Add(obj);
        };
        UniFileScriptBehaviour.unregisterScriptDelegate = (path, obj) => {
            if (scripts.ContainsKey(path) == false)
                return;
            scripts[path].Remove(obj);
        };
#endif
    }

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        if (Application.isPlaying == false)
            return;

        var changes = new List<string>();
        foreach (var asset in importedAssets)
        {
            if (asset.EndsWith(".csx") == false)
                continue;
            
            if (scripts.ContainsKey(asset))
            {
                foreach (var obj in scripts[asset])
                {
                    obj.ReloadScript();
                }

                AutoLoadIndicator.applied = true;
            }
        }
    }
}
