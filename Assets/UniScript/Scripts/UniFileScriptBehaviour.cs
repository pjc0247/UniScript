using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniFileScriptBehaviour : UniScriptBehaviour
{
    public static Action<string, UniFileScriptBehaviour> registerScriptDelegate;
    public static Action<string, UniFileScriptBehaviour> unregisterScriptDelegate;

    public UnityEngine.Object script;
    public string scriptPath;

    public void Awake()
    {
        if (string.IsNullOrEmpty(scriptPath))
            return;
        if (registerScriptDelegate != null)
            registerScriptDelegate.Invoke(scriptPath, this);

        ReloadScript();
    }
    public void ReloadScript()
    {
        Bind(File.ReadAllText(scriptPath));
    }
    void OnDestroy()
    {
        if (string.IsNullOrEmpty(scriptPath))
            return;
        if (unregisterScriptDelegate != null)
            unregisterScriptDelegate.Invoke(scriptPath, this);
    }
}
