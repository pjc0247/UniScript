using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModScript : UniFileScriptBehaviour
{
    private bool hasOnInteraction;

    protected override void BuildFlags()
    {
        base.BuildFlags();

        hasOnInteraction = HasMethod(nameof(OnInteraction));
    }

    void OnInteraction(InteractionData data)
    {
        if (hasOnInteraction)
            instance.Invoke(nameof(OnInteraction), data);
    }
}
