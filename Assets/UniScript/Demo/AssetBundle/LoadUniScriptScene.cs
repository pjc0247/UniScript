using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniScript;

public class LoadUniScriptScene : MonoBehaviour
{
    void Start()
    {
        // If you face 404 not found error, 
        // you should build AssetBundles first.
        //
        // Click `Build/BuildAssetBundle` menu on Unity Editor.
        // (or please take a look `Editor/CreateAssetBundle.cs`)
        UniAssetBundle.LoadUniScriptScene(
            "file:///Packs/testaa_scene",
            "file:///Packs/testaa_script");
    }
}
