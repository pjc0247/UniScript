using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : UniFileScriptBehaviour
{
    void Update()
    {
        transform.Translate(transform.forward * Time.deltaTime);
    }
}
