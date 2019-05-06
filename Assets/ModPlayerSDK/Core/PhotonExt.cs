using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhotonExt
{
    public static T SafeGet<T>(this ExitGames.Client.Photon.Hashtable _this, string key, T _default)
    {
        if (_this.ContainsKey(key) == false)
            return _default;
        return (T)_this[key];
    }
    public static bool Is<T>(this ExitGames.Client.Photon.Hashtable _this, string key, T expected)
        where T : class
    {
        return _this.SafeGet(key, default(T)) == expected;
    }
}
