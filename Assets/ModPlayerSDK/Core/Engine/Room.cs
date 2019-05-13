using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class Room : MonoBehaviourPunCallbacks
{
    public static string type => (string)PhotonNetwork.CurrentRoom.CustomProperties["type"];

    public static SortedDictionary<string, PPlayer> players = new SortedDictionary<string, PPlayer>();
    public static PPlayer[] alivePlayers
    {
        get
        {
            return players
                .Select(x => x.Value)
                .Where(x => x.isDead == false)
                .ToArray();
        }
    }

    public static PhotonHashtable CustomProperties
    {
        get
        {
            return PhotonNetwork.CurrentRoom.CustomProperties;
        }
    }

    public static Action onContextUpdated;
    public static PlayContext context
    {
        get {
            return (PlayContext)PhotonNetwork.CurrentRoom.CustomProperties["context"];
        }
    }
    public static void UpdateContext()
    {
        PhotonNetwork.CurrentRoom.SetCustomProperties(new PhotonHashtable()
        {
            ["context"] = context
        });
    }

    public static T GetGameContext<T>()
    {
        try
        {
            return (T)PhotonNetwork.CurrentRoom.CustomProperties["game"];
        }
        catch(InvalidCastException e)
        {
            Debug.LogError($"InvalidCastException: Actual {PhotonNetwork.CurrentRoom.CustomProperties["game"].GetType()} / Tried {typeof(T)}");
            throw e;
        }
    }
    public static void UpdateGameContext<T>(T o)
    {
        PhotonNetwork.CurrentRoom.SetCustomProperties(new PhotonHashtable()
        {
            ["game"] = o
        });
    }
    public static void ResetGameContext<T>()
        where T : new()
    {
        UpdateGameContext(new T());
    }

    public static bool isPlaying
    {
        get
        {
            if (PhotonNetwork.CurrentRoom == null) return false;
            return CustomProperties.SafeGet<bool>("isPlaying", false);
        }
    }

    public static bool isChatAvaliable
    {
        get {
            if (PhotonNetwork.CurrentRoom == null) return true;
            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("isChatAvaliable") == false)
                return true;
            return (bool)PhotonNetwork.CurrentRoom.CustomProperties["isChatAvaliable"];
        }
        set
        {
            if (PhotonNetwork.IsMasterClient == false) return;
            if (PhotonNetwork.CurrentRoom == null) return;
            PhotonNetwork.CurrentRoom.SetCustomProperties(new PhotonHashtable() {
                ["isChatAvaliable"] = value
            });
        }
    }
    public static bool isControllable
    {
        get
        {
            if (PhotonNetwork.CurrentRoom == null) return true;
            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("isControllable") == false)
                return true;
            return (bool)PhotonNetwork.CurrentRoom.CustomProperties["isControllable"];
        }
        set
        {
            if (PhotonNetwork.IsMasterClient == false) return;
            if (PhotonNetwork.CurrentRoom == null) return;
            PhotonNetwork.CurrentRoom.SetCustomProperties(new PhotonHashtable() {
                ["isControllable"] = value
            });
        }
    }
    public static bool isPvpAvaliable
    {
        get
        {
            if (PhotonNetwork.CurrentRoom == null) return true;
            return Room.CustomProperties.SafeGet("isPvpAvaliable", false);
        }
        set
        {
            if (PhotonNetwork.IsMasterClient == false) return;
            if (PhotonNetwork.CurrentRoom == null) return;
            PhotonNetwork.CurrentRoom.SetCustomProperties(new PhotonHashtable()
            {
                ["isPvpAvaliable"] = value
            });
        }
    }
    public static bool is3rdPersonCam
    {
        get
        {
            if (PhotonNetwork.CurrentRoom == null) return true;
            return Room.CustomProperties.SafeGet("is3rdPersonCam", false);
        }
        set
        {
            if (PhotonNetwork.IsMasterClient == false) return;
            if (PhotonNetwork.CurrentRoom == null) return;
            PhotonNetwork.CurrentRoom.SetCustomProperties(new PhotonHashtable() {
                ["is3rdPersonCam"] = value
            });
        }
    }

    public static void SetCustomProperties(Dictionary<string, object> p)
    {
        if (PhotonNetwork.IsMasterClient == false) return;
        if (PhotonNetwork.CurrentRoom == null) return;

        var photonHashtable = new PhotonHashtable();
        foreach (var pair in p)
            photonHashtable[pair.Key] = pair.Value;
        PhotonNetwork.CurrentRoom.SetCustomProperties(photonHashtable);
    }

    public override void OnRoomPropertiesUpdate(PhotonHashtable changes)
    {
        if (changes.ContainsKey("context"))
            onContextUpdated?.Invoke();
    }
}

