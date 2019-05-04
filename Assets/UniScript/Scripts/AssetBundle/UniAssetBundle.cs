using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public partial class UniAssetBundle : MonoBehaviour
{
    public static UniAssetBundle instance;

    public static void LoadUniScriptScene(string sceneUrl, string assetUrl)
    {
        EnsureInstance();
        instance.StartCoroutine(instance._LoadUniScriptScene(sceneUrl, assetUrl));
    }
    private static void EnsureInstance()
    {
        if (instance != null) return;

        var gobj = new GameObject("UniAssetBundle");
        gobj.AddComponent<UniAssetBundle>();
    }

    private void Awake()
    {
        instance = this;
    }
    private IEnumerator _LoadUniScriptScene(string sceneUrl, string assetUrl)
    {
        var sceneRequest = UnityWebRequestAssetBundle.GetAssetBundle(sceneUrl);
        var assetRequest = UnityWebRequestAssetBundle.GetAssetBundle(assetUrl);

        var sceneReq = sceneRequest.SendWebRequest();
        var assetReq = assetRequest.SendWebRequest();

        yield return sceneReq;
        yield return assetReq;

        if (string.IsNullOrEmpty(sceneRequest.error) == false)
            Debug.LogError(sceneRequest.error);
        if (string.IsNullOrEmpty(assetRequest.error) == false)
            Debug.LogError(assetRequest.error);

        var sceneBundle = DownloadHandlerAssetBundle.GetContent(sceneRequest);
        var assetBudnle = DownloadHandlerAssetBundle.GetContent(assetRequest);

        UniFileScriptBehaviour.LoadScriptBundle(assetBudnle.LoadAsset<TextAsset>("uniscript_monolith"));
        Debug.Log("[LoadScene] " + sceneBundle.GetAllScenePaths()[0]);
        SceneManager.LoadScene(sceneBundle.GetAllScenePaths()[0]);
    }
}
