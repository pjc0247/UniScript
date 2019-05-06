using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

public class Bootstrap : MonoBehaviourPunCallbacks
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        var gobj = new GameObject("ModPlayerBootstrap");
        gobj.AddComponent<Bootstrap>();
        gobj.AddComponent<PlayerSpawn>();
        gobj.AddComponent<InputManager>();
        DontDestroyOnLoad(gobj);
    }

    void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("[Connected]");

        PhotonNetwork.LocalPlayer.SetCustomProperties(new PhotonHashtable() {
            ["character"] = "voxelgirl_black"
        });
        PhotonNetwork.NickName = "EditorPlayer";

        CreateRoom("test", RoomType.Team, null);
    }

    public void CreateRoom(string name, string roomType, string password)
    {
        var opt = new RoomOptions();
        opt.PublishUserId = true;
        opt.IsVisible = true;
        opt.MaxPlayers = 4;
        opt.CustomRoomPropertiesForLobby = new string[] { "name", "type", "version", "password" };
        opt.CustomRoomProperties = new PhotonHashtable() {
            ["name"] = name,
            ["type"] = roomType,
            ["password"] = password,
            ["version"] = Application.version,
            ["isChatAvaliable"] = true,
            ["isPvpAvaliable"] = true,
            ["is3rdPersonCam"] = roomType == RoomType.Chat || roomType == RoomType.Cinema ? true : false
        };
        PhotonNetwork.CreateRoom(null, opt, TypedLobby.Default);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("[OnJoinedRoom]");
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("[OnJoinRoomFailed] " + message);
    }
}
