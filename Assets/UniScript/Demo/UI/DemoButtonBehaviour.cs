using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoButtonBehaviour : UniFileScriptBehaviour
{
    public void OnClickButton()
    {
        instance.Invoke("OnClick");
    }
}
