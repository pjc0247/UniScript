using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public float delay = 2;

    void Awake()
    {
        Destroy(gameObject, delay);
    }
}
