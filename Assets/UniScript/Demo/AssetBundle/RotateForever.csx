using System;
using System.Collections;
using UnityEngine;

class RotateForever : UniFileScriptBehaviour
{
    public void OnBind()
    {
    }
    public void Update()
    {
        transform.Rotate(new Vector3(10, 10, 10));
    }
}