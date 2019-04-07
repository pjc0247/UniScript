using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoBehaviour : UniScriptBehaviour
{
    public Transform target;

    void Awake()
    {
        Bind(@"
using UnityEngine;

class DemoScript : DemoBehaviour {
    public void Update() {
        target.position += new Vector3(0, 0, 1);
    }
}");
    }

    public void OnClickButton()
    {

    }
}
