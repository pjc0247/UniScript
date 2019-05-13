using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform target;

    private float distance;
    private int overlap = 0;
    private Transform initialCamPosition;

    void OnEnable()
    {
        distance = Vector3.Distance(transform.position, target.position);

        var go = new GameObject("initialCamPosition");
        go.transform.SetParent(transform);
        go.transform.localPosition = Vector3.zero;
        go.transform.SetParent(transform.parent);
        initialCamPosition = go.transform;
    }
    void Update()
    {
        transform.LookAt(target);

        var player = transform.parent.parent.position + new Vector3(0, 2.0f, 0);
        var forward = (player - initialCamPosition.position);
        initialCamPosition.localEulerAngles = transform.localEulerAngles;
        RaycastHit hit;
        if (Physics.Raycast(player, -forward,
                //        if (Physics.Raycast(initialCamPosition.position, initialCamPosition.forward,
                out hit, 5)){

            if (hit.collider.gameObject.GetComponent<PPlayer>())
                return;

            Debug.Log(hit.collider.gameObject + " /  " + hit.distance);

            var pos = transform.localPosition;
            //pos.z = -6.33f + hit.distance + 0.1f;
            pos.z = -hit.distance;
            transform.localPosition = pos;
        }
        else  {
            var pos = transform.localPosition;
            pos.z = -6.33f;
            transform.localPosition = pos;
        }
    }

    /*
    void OnTriggerEnter(Collider other)
    {
        overlap++;

        StartCoroutine(DistanceTo(distance / 2));
        StartCoroutine(CheckAndRestore());
    }
    void OnTriggerExit(Collider other)
    {
        overlap--;

    }
    */
    IEnumerator DistanceTo(float f)
    {
        var pos = initialCamPosition.position - target.position;
        pos = pos.normalized;

        pos = initialCamPosition.position + pos * f;

        for (int i = 0; i < 30; i++)
        {
            transform.position += (pos - transform.position) * 0.1f;
            yield return null;
        }
    }
}
