using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModPlayerSDK.Model
{
    public class ModApp
    {
        public string id;
        public string owner;
        public string name;
        public string thumbnail_url;
        public string preview_urls;
        public ModBuild[] build;

        // Runtime properties
        public Texture2D thumbnail;
        public ModBuild last_build
        {
            get
            {
                if (build == null || build.Length == 0)
                    return null;
                return build[build.Length - 1];
            }
        }
        public bool has_build
        {
            get
            {
                if (build == null || build.Length == 0)
                    return false;
                return true;
            }
        }

        public string[] GetPreviewUrls()
        {
            return preview_urls.Split(',');
        }
    }
}
