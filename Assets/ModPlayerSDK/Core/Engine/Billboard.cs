using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class Billboard : MonoBehaviour
{
    void Update()
    {
        //transform.LookAt(Camera.main.transform);
        //transform.localEulerAngles += new Vector3(0, 180, 0);
        Vector3 v = Camera.main.transform.position - transform.position;

        v.x = v.z = 0.0f;

        transform.LookAt(Camera.main.transform.position - v);
        transform.Rotate(0, 180, 0);

        /*
        transform.localEulerAngles = new Vector3(
            transform.localEulerAngles.x,
            0,
            transform.localEulerAngles.z);
            */
        return;

#if UNITY_EDITOR
        //if (Application.isPlaying)
            //transform.LookAt(UnityEditor.SceneView.GetAllSceneCameras()[0].transform);
#else
        transform.localEulerAngles = new Vector3(
            0, -transform.root.localEulerAngles.y, 0);
#endif
    }
}
