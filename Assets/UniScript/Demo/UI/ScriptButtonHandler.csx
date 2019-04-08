using System;
using UnityEngine;

class ScriptButtonHandler : DemoButtonBehaviour {

    public void OnClick() {
        Debug.Log("[OnClick]");

        DemoPopupBehaviour.instance.Show("Button Clicked!!");
    }
}