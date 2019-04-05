using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UniScriptConfigEditor : EditorWindow
{
    public static readonly string Path = "Assets/UniScript/config.asset";

    [MenuItem("UniScript/Config")]
    public static void ConfigUniScript()
    {
        var asset = AssetDatabase.LoadAssetAtPath<UniScriptConfig>(Path);
        if (asset == null)
        {
            asset = ScriptableObject.CreateInstance<UniScriptConfig>();
            AssetDatabase.CreateAsset(asset, Path);
            AssetDatabase.SaveAssets();
        }
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}
