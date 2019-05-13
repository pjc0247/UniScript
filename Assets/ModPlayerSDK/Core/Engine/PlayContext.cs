using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;

[Serializable]
public class PlayContext
{
    public int life   = 5;
    public int clear  = 0;

    public Dictionary<string, int> awards = new Dictionary<string, int>();
    public Dictionary<string, int> points = new Dictionary<string, int>();
    public HashSet<string> history = new HashSet<string>();

    [RuntimeInitializeOnLoadMethod]
    private static void RegisterType()
    {
    }

    public void AddPoint(string pid, int n)
    {
        if (points.ContainsKey(pid) == false)
            points[pid] = n;
        else
            points[pid] += n;
    }
    public void AwardCoin(string pid, int n)
    {
        if (awards.ContainsKey(pid) == false)
            awards[pid] = n;
        else
            awards[pid] += n;
    }

    public int GetAwards(string pid)
    {
        if (awards.ContainsKey(pid) == false)
            return 0;
        return awards[pid];
    }

    public PPlayer GetMVP()
    {
        var pid = points
            .OrderBy(x => x.Value)
            .First()
            .Key;
        return Room.players[pid];
    }
}
