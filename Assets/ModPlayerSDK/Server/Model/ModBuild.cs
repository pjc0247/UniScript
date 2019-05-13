using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModPlayerSDK.Model
{
    public class ModBuild
    {
        public string title;
        public string version;
        public string description;

        public string scene_url;
        public string script_url;

        public FirebaseTimestamp created_at;

        public DateTime GetCreatedAt()
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(created_at._seconds);
        }
    }
    public class FirebaseTimestamp
    {
        public long _seconds;
    }
}
