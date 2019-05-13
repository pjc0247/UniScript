using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

namespace UniScript
{
    public class ModResource
    {
        private static Dictionary<string, ResourceMapItem> resources 
            = new Dictionary<string, ResourceMapItem>();

        public static void LoadResourceMap(TextAsset asset, AssetBundle assetBundle)
        {
            if (asset == null)
                throw new ArgumentNullException(nameof(asset));

            Debug.Log(asset.text);

            var map = JsonConvert.DeserializeObject<ResourceMap>(asset.text);
            foreach (var kv in map.resources)
            {
                kv.Value.assetBundle = assetBundle;
                resources[kv.Value.path] = kv.Value;
            }
        }

        public static T Load<T>(string path)
            where T : UnityEngine.Object
        {
            ResourceMapItem item = null;
            if (resources.TryGetValue(path, out item) == false)
            {
#if UNITY_EDITOR
                return Resources.Load<T>(path);
#else
                return null;
#endif
            }

            return item.assetBundle.LoadAsset<T>(item.name);
        }
    }
}
