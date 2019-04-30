using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniHttpScriptBehaviour : UniScriptBehaviour
{
    public string scriptUrl;

    public void Awake()
    {
        if (string.IsNullOrEmpty(scriptUrl))
            return;

        StartCoroutine(ReloadScript());
    }
    IEnumerator ReloadScript()
    {
        var www = new WWW(scriptUrl);
        yield return www;

        if (string.IsNullOrEmpty(www.error))
            Bind(www.text);
        else
            Debug.LogError("[LoadError] " + www.error);
    }
}
