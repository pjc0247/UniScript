using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateAssetBundle
{
    [MenuItem("Build/BuildAssetBundle")]
    public static void BuildAssetBundle()
    {
        UniAssetBundle.BuildUniScriptScene("testbundle", "testbundle_script");
    }
}
