using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace UniScript
{
    public partial class UniAssetBundle : MonoBehaviour
    {
        public static UniAssetBundle instance;

        public static void LoadUniScriptScene(string sceneUrl, string assetUrl, bool additive = false)
        {
            EnsureInstance();
            instance.StartCoroutine(instance._LoadUniScriptScene(sceneUrl, assetUrl, additive));
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
        private IEnumerator _LoadUniScriptScene(string sceneUrl, string assetUrl, bool additive = false)
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
            var assetBundle = DownloadHandlerAssetBundle.GetContent(assetRequest);

            var scriptMonolith = assetBundle.LoadAsset<TextAsset>("uniscript_monolith");
            var resourcesMap = assetBundle.LoadAsset<TextAsset>("uniscript_resource_map");

            if (scriptMonolith == null || resourcesMap == null)
                throw new InvalidOperationException("Not a valid UniScript scene");

            UniFileScriptBehaviour.LoadScriptBundle(scriptMonolith);
            ModResource.LoadResourceMap(resourcesMap, assetBundle);

            Debug.Log("[LoadScene] " + sceneBundle.GetAllScenePaths()[0]);
            SceneManager.LoadScene(sceneBundle.GetAllScenePaths()[0],
                additive ? LoadSceneMode.Additive : LoadSceneMode.Single);
        }
    }
}
