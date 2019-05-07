﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Slowsharp;

public sealed class RuntimeScript
{
    public static Action<string, int> showDebugger;

    public static CScript runner;
    public static DumpSnapshot dump;

    public static void Attach(GameObject obj, string script)
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj));
        if (string.IsNullOrEmpty(script))
            throw new ArgumentException(nameof(script));

        var uniScript = obj.AddComponent<UniScriptBehaviour>();
        uniScript.Bind(script);
    }

    public static void Break()
    {
        dump = runner.GetDebuggerDump();

#if UNITY_EDITOR
        if (showDebugger == null)
            return;

        var src = Convert.ToBase64String(
            Encoding.UTF8.GetBytes(dump.methodSrc));
        showDebugger.Invoke(src, dump.breakLine);
#endif
    }
    public static void DumpLocals()
    {
        dump = runner.GetDebuggerDump();

        var sb = new StringBuilder("{\r\n");
        foreach (var local in dump.locals)
        {
            sb.AppendLine($"  <b>{local.Key}</b>: {local.Value}");
        }
        sb.Append("}");

        Debug.Log(sb.ToString());
    }

    public static string Eval(string src)
    {
        var ret = runner.Eval(src);
        if (ret == null) return "(null)";
        return ret.ToString();
    }
}