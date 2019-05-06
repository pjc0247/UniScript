using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public enum FxType
{
    None,

    Punch, SurfaceHit,

    Explosion,
    StarExplosion
}
public class Fx : MonoBehaviourPun
{
    private static Fx instance;

    public static GameObject Punch;
    public static GameObject SurfaceHit;
    public static GameObject StarExplosion;
    public static GameObject Explosion;

    [RuntimeInitializeOnLoadMethod()]
    private static void Init()
    {
        Punch = Resources.Load<GameObject>("Fx/Punch");
        SurfaceHit = Resources.Load<GameObject>("Fx/SurfaceHit");
        StarExplosion = Resources.Load<GameObject>("Fx/StarExplosion");
        Explosion = Resources.Load<GameObject>("Fx/Explosion");
    }

    public static GameObject Create(GameObject fx, Vector3 position)
    {
        var p = GameObject.Instantiate(fx);
        p.transform.position = position;
        return p;
    }
    public static void CreateAll(FxType type, Vector3 position)
    {
        if (type == FxType.None) return;

        instance.photonView.RPC(
            nameof(RpcCreateFx), RpcTarget.All,
            type, position);
    }

    void Awake()
    {
        instance = this;
    }

    [PunRPC]
    private void RpcCreateFx(FxType type, Vector3 position)
    {
        GameObject fx = null;
        switch (type)
        {
            case FxType.Punch: fx = Punch; break;
            case FxType.SurfaceHit: fx = SurfaceHit; break;
            case FxType.StarExplosion: fx = StarExplosion; break;
            case FxType.Explosion: fx = Explosion; break;
        }

        if (fx != null)
            Create(fx, position);
    }
}
