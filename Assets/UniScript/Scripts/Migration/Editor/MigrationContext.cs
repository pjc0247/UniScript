using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEditor;

namespace UniScript
{
    [Serializable]
    public class MigrationContext
    {
        public string scenePath;

        public List<MigratedPrefabData> prefabs = new List<MigratedPrefabData>();
    }
    public class MigratedPrefabData
    {
        public string path;
    }
}
