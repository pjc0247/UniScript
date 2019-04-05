using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniScriptInitializer : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        var config = Resources.Load<UniScriptConfig>("uniscriptconfig");
        if (config == null)
            Debug.LogWarning("No valid config found");

        Debug.Log(config.timeoutMS);
    }
}
