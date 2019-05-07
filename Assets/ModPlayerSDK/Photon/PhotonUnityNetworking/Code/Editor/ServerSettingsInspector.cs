// ----------------------------------------------------------------------------
// <copyright file="ServerSettingsInspector.cs" company="Exit Games GmbH">
//   PhotonNetwork Framework for Unity - Copyright (C) 2018 Exit Games GmbH
// </copyright>
// <summary>
//   This is a custom editor for the ServerSettings scriptable object.
// </summary>
// <author>developer@exitgames.com</author>
// ----------------------------------------------------------------------------

using UnityEditor;
using UnityEngine;

using Photon.Pun;

using ExitGames.Client.Photon;

[CustomEditor(typeof (ServerSettings))]
public class ServerSettingsInspector : Editor
{
    private string versionPhoton;

    public void Awake()
    {
        this.versionPhoton = System.Reflection.Assembly.GetAssembly(typeof(PhotonPeer)).GetName().Version.ToString();
    }


    public override void OnInspectorGUI()
    {

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(new GUIContent("Version", "Version of PUN and Photon3Unity3d.dll."));
        EditorGUILayout.LabelField("Pun: " + PhotonNetwork.PunVersion + " Photon lib: " + this.versionPhoton);
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(PhotonGUI.HelpIcon, GUIStyle.none))
        {
            Application.OpenURL(PhotonEditor.UrlPunSettings);
        }
        EditorGUILayout.EndHorizontal();



        SerializedProperty settingsSs = this.serializedObject.FindProperty("ShowSettings");

      
        ServerSettings settings = this.target as ServerSettings;

        bool _newShowSettings = EditorGUILayout.Foldout(settingsSs.boolValue, new GUIContent("Settings", "Core Photon Server/Cloud settings."));
        if (_newShowSettings != settings.ShowSettings)
        {
            settingsSs.boolValue = _newShowSettings;
        }

       

        if (settingsSs.boolValue)
        {
            SerializedProperty settingsSp = this.serializedObject.FindProperty("AppSettings");

            EditorGUI.indentLevel++;

            //Realtime APP ID
            BuildAppIdField(settingsSp.FindPropertyRelative("AppIdRealtime"));


            if (PhotonEditorUtils.HasChat)
            {
                BuildAppIdField(settingsSp.FindPropertyRelative("AppIdChat"));
            }
            if (PhotonEditorUtils.HasVoice)
            {
                BuildAppIdField(settingsSp.FindPropertyRelative("AppIdVoice"));
            }

            EditorGUILayout.PropertyField(settingsSp.FindPropertyRelative("AppVersion"));
            EditorGUILayout.PropertyField(settingsSp.FindPropertyRelative("UseNameServer"), new GUIContent("Use Name Server", "Photon Cloud requires this checked.\nUncheck for Photon Server SDK (OnPremise)."));
            EditorGUILayout.PropertyField(settingsSp.FindPropertyRelative("FixedRegion"), new GUIContent("Fixed Region", "Photon Cloud setting, needs a Name Server.\nDefine one region to always connect to.\nLeave empty to use the best region from a server-side region list."));
            EditorGUILayout.PropertyField(settingsSp.FindPropertyRelative("Server"), new GUIContent("Server", "Typically empty for Photon Cloud.\nFor Photon OnPremise, enter your host name or IP. Also uncheck \"Use Name Server\" for older Photon OnPremise servers."));
            EditorGUILayout.PropertyField(settingsSp.FindPropertyRelative("Port"), new GUIContent("Port","Use 0 for Photon Cloud.\nOnPremise uses 5055 for UDP and 4530 for TCP."));
            EditorGUILayout.PropertyField(settingsSp.FindPropertyRelative("Protocol"), new GUIContent("Protocol", "Use UDP where possible.\nWSS works on WebGL and Xbox exports.\nDefine WEBSOCKET for use on other platforms."));
            EditorGUILayout.PropertyField(settingsSp.FindPropertyRelative("EnableLobbyStatistics"), new GUIContent("Enable Lobby Statistics", "When using multiple room lists (lobbies), the server can send info about their usage."));
            EditorGUILayout.PropertyField(settingsSp.FindPropertyRelative("NetworkLogging"), new GUIContent("Network Logging", "Log level for the Photon libraries."));
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("StartInOfflineMode"), new GUIContent("Start In Offline Mode", "Simulates an online connection.\nPUN can be used as usual."));
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("PunLogging"), new GUIContent("PUN Logging", "Log level for the PUN layer."));
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("EnableSupportLogger"), new GUIContent("Enable Support Logger", "Logs additional info for debugging.\nUse this when you submit bugs to the Photon Team."));
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("RunInBackground"), new GUIContent("Run In Background", "Enables apps to keep the connection without focus. Android and iOS ignore this."));

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(new GUIContent("Best Region Preference", "Clears the Best Region of the editor.\n.Best region is used if Fixed Region is empty."));
        if (GUILayout.Button("Reset", EditorStyles.miniButton))
        {
            ServerSettings.ResetBestRegionCodeInPreferences();
        }

        if (GUILayout.Button("Edit Regions WhiteList", EditorStyles.miniButton))
        {
            Application.OpenURL("https://dashboard.photonengine.com/en-US/App/RegionsWhitelistEdit/" + PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime);

        }

        EditorGUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Rpc Tools");
        if (GUILayout.Button("Refresh RPCs",EditorStyles.miniButton))
        {
            PhotonEditor.UpdateRpcList();
            this.Repaint();
        }
        if (GUILayout.Button("Clear RPCs",EditorStyles.miniButton))
        {
            PhotonEditor.ClearRpcList();
        }
        if (GUILayout.Button("Log HashCode",EditorStyles.miniButton))
        {
            Debug.Log("RPC-List HashCode: " + this.RpcListHashCode() + ". Make sure clients that send each other RPCs have the same RPC-List.");
        }

        GUILayout.EndHorizontal();

        SerializedObject sObj = new SerializedObject(target);
        SerializedProperty sRpcs = sObj.FindProperty("RpcList");
        EditorGUILayout.PropertyField(sRpcs, true);

        if (EditorGUI.EndChangeCheck())
        {
            this.serializedObject.ApplyModifiedProperties();
        }

    }


    private int RpcListHashCode()
    {
        // this is a hashcode generated to (more) easily compare this Editor's RPC List with some other
        int hashCode = PhotonNetwork.PhotonServerSettings.RpcList.Count + 1;
        foreach (string s in PhotonNetwork.PhotonServerSettings.RpcList)
        {
            int h1 = s.GetHashCode();
            hashCode = ((h1 << 5) + h1) ^ hashCode;
        }

        return hashCode;
    }


    private void BuildAppIdField(SerializedProperty property)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(property);
        string appId = property.stringValue;
        string url = "https://dashboard.photonengine.com/en-US/PublicCloud";
        if (!string.IsNullOrEmpty(appId))
        {
            url = string.Format("https://dashboard.photonengine.com/en-US/App/Manage/{0}", appId);
        }
        if (GUILayout.Button("Dashboard", EditorStyles.miniButton, GUILayout.Width(70)))
        {
            Application.OpenURL(url);
        }
        EditorGUILayout.EndHorizontal();
    }
}