using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ModPlayerSDK
{
    public class BundleResources 
    {
        public static void SetAssetBundleAllResources(string bundleName)
        {
            foreach (var path in AssetDatabase.GetAllAssetPaths())
            {
                if (path.Contains("Resources/") == false)
                    continue;
                // Scene must not be bundled together
                if (path.EndsWith(".unity"))
                    continue;

                AssetImporter.GetAtPath(path)
                    .SetAssetBundleNameAndVariant(bundleName, "");
            }
        }
    }
}