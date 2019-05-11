using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawn : MonoBehaviourPunCallbacks
{
    private GameObject player;

    public override void OnJoinedRoom()
    {
        player = PhotonNetwork.Instantiate("Players/Player",
            SpawnPoint.point,
            Quaternion.identity, 0);
    }
    void OnDestroy()
    {
        if (player != null)
            PhotonNetwork.Destroy(player);
    }
}
