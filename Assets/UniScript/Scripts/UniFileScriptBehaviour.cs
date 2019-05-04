using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniFileScriptBehaviour : UniScriptBehaviour
{
    public static Action<string, UniFileScriptBehaviour> registerScriptDelegate;
    public static Action<string, UniFileScriptBehaviour> unregisterScriptDelegate;

    private static bool isScriptBundleLoaded = false;
    private static Dictionary<string, string> scripts = new Dictionary<string, string>();

    public UnityEngine.Object script;
    [HideInInspector]
    public string scriptPath;

    #region INTERNAL_USE_ONLY
    public static void LoadScriptBundle(TextAsset asset)
    {
        Debug.Log(asset.text);

        scripts = ((Dictionary<string, object>)UniScriptInternal.MiniJSON.Json.Deserialize(asset.text))
            .ToDictionary(x => x.Key, x => (string)x.Value);

        Debug.Log($"[UniFileScript] Loaded {scripts.Count} scripts");
    }
    private static void LoadScriptBundle()
    {
        if (isScriptBundleLoaded) return;

        var monolith = Resources.Load<TextAsset>("uniscript/monolith");
        if (monolith == null)
        {
            Debug.LogWarning("No monolith.txt found");
            return;
        }

        LoadScriptBundle(monolith);
    }
    public static void UpdateScript(string path, string src)
    {
        scripts[path] = src;
    }
    #endregion

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
        var src = "";

#if UNITY_EDITOR
        if (scripts.ContainsKey(scriptPath))
            src = scripts[scriptPath];
        else
            src = File.ReadAllText(scriptPath);
#else
        if (isScriptBundleLoaded == false)
            LoadScriptBundle();
        src = scripts[scriptPath];
#endif

        Bind(src);
    }
    void OnDestroy()
    {
        if (string.IsNullOrEmpty(scriptPath))
            return;
        if (unregisterScriptDelegate != null)
            unregisterScriptDelegate.Invoke(scriptPath, this);
    }

    private string GetResourcesRelativePath()
    {
        var tokens = scriptPath.Split(new string[] { "Resources/" }, 
            2, StringSplitOptions.RemoveEmptyEntries);

        if (tokens.Length == 0)
            throw new ArgumentException("Script doest not located in Resources directory");

        return tokens[1].Split('.')[0];
    }
}
