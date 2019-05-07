using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UniScriptMenu
{
    [MenuItem("Assets/Create/C# Uni Script (.csx)", priority = 81)]
    public static void CreateCsx()
    {
        var path = Path.Combine(AssetDatabase.GetAssetPath(Selection.activeObject), "NewScript.csx");
        File.WriteAllText(
            path,
            @"using System;
using System.Collections;
using UnityEngine;

class NewScript : UniFileScriptBehaviour
{
    public void OnBind()
    {
    }
}");

        AssetDatabase.ImportAsset(path);
        var o = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
        ProjectWindowUtil.ShowCreatedAsset(o);
    }
}
