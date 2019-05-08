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
        public ModBuild[] builds;

        // Runtime properties
        public Texture2D thumbnail;
        public ModBuild last_build
        {
            get
            {
                if (builds == null || builds.Length == 0)
                    return null;
                return builds[builds.Length - 1];
            }
        }
    }
}
