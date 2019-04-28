using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Neccessary to handle keyboard in HTML other elements.
/// https://docs.unity3d.com/Manual/webgl-input.html
/// </summary>
public class EnableWebGLKeyInput : MonoBehaviour
{
    void Start()
    {
#if UNITY_WEBGL
        WebGLInput.captureAllKeyboardInput = false;
#endif
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
            WebGLInput.captureAllKeyboardInput = true;
        else
            WebGLInput.captureAllKeyboardInput = false;
    }
}
