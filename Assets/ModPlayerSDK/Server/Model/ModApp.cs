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

        public Texture2D thumbnail;
    }
}
