using System;
using System.Collections;
using UnityEngine;

class NewScript : ModScript
{
    public void OnInteraction(InteractionData data) {
        Destroy(gameObject);
    }
}