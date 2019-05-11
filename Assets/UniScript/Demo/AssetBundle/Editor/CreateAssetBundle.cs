using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using UniScript;

public class CreateAssetBundle
{
    [MenuItem("Build/BuildAssetBundle")]
    public static void BuildAssetBundle()
    {
        UniAssetBundle.BuildUniScriptScene("testbundle_scene", "testbundle_script");
    }
}
