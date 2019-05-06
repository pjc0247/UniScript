using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateF : UniFileScriptBehaviour
{
    public Transform target;
    public float speed = 1;

    public void Update()
    {
        target.Rotate(new Vector3(speed, speed, speed));
    }
}
