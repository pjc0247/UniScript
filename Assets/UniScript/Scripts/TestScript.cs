using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : UniScriptBehaviour
{
    protected override void Awake()
    {
        base.Awake();

        Bind(@"
using System;
using UnityEngine;

public class TestScript2 : TestScript {
    public void Update() {  
        if (Input.GetKeyDown(KeyCode.Space))  {
            transform.position += new Vector3(0, 0, 1);
            Destroy(this);
        }

var config = (UniScriptConfig)Resources.Load(""uniscriptconfig"");

Debug.Log(config.timeoutMS);
    }
    public void OnEnable(){
        transform.position += new Vector3(0, 0, 1);
    }
}
");
    }
}
