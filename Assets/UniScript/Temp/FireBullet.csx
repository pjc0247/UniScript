using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniScript;

public class FireBullet : UniFileScriptBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var b = Instantiate(
                ModResource.Load<GameObject>("Bullet"));
            b.transform.position = transform.position;
        }
    }
}
