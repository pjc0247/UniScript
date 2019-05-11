using UnityEngine;
using System.Collections;

using Photon.Pun;

public class ImpactReceiver : MonoBehaviourPunCallbacks
{
    float mass = 3.0F; // defines the character mass
    Vector3 impact = Vector3.zero;
    private CharacterController character;

    void Awake()
    {
        character = GetComponent<CharacterController>();
    }
    public bool UpdateImpact()
    {
        if (impact.magnitude > 0.2F)
        {
            character.SimpleMove(impact * Time.deltaTime * 20);
            impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
            return true;
        }
        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
        return false;
    }
    public void AddImpact(Vector3 dir, float force)
    {
        transform.localEulerAngles = new Vector3(0, 
            -Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg, 0);

        dir.Normalize();
        if (dir.y < 0) dir.y = -dir.y;
        impact += dir.normalized * force / mass;
    }
}