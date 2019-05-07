using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            "file:///Packs/testbundle",
            "file:///Packs/testbundle_script");
    }
}
