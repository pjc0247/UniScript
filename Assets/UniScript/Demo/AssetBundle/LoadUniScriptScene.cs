using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadUniScriptScene : MonoBehaviour
{
    void Start()
    {
        UniAssetBundle.LoadUniScriptScene(
            "file:///Packs/testbundle",
            "file:///Packs/testbundle_script");
    }
}
