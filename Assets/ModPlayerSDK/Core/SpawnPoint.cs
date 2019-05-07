using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpawnPoint : MonoBehaviour
{
    private static SpawnPoint instance;
    public static Vector3 point => instance.transform.position;

    void Awake()
    {
        instance = this;
    }

    IEnumerator Start()
    {
        while (PPlayer.LocalPlayer == null)
            yield return null;

    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "startpoint");
    }
#endif
}
