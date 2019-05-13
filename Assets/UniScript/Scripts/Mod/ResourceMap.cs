using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniScript
{
    [Serializable]
    public class ResourceMap
    {
        public Dictionary<string, ResourceMapItem> resources;
    }
    [Serializable]
    public class ResourceMapItem
    {
        public string name;
        public string path;

        public AssetBundle assetBundle;
    }
}