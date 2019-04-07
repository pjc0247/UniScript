using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : UniScriptBehaviour
{
    void Awake()
    {
        Bind(@"
using System;
using UnityEngine;

public class TestScript2 : TestScript {
    public void Update() {  
        var a = 11;
        if (Input.GetKeyDown(KeyCode.Space))  {
            CallStackTest(0);
            transform.position += new Vector3(0, 0, 1);
            Destroy(this);
        }
        var config = (UniScriptConfig)Resources.Load(""uniscriptconfig"");
        Debug.Log(config.timeoutMS);
    }
    void CallStackTest(int depth) {
        var b = 11;
        var c = 12341;
        var _null = null;

        if (depth == 3){
            UniScript.DumpLocals();
            UniScript.Break();
        }
        else
            CallStackTest(depth + 1);

        var shouldNotBeVisible = 44;
    }
    public void OnEnable(){
        transform.position += new Vector3(0, 0, 1);
    }
}
");
    }
}
