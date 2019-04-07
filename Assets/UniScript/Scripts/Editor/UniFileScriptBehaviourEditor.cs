using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UniFileScriptBehaviour))]
public class UniFileScriptBehaviourEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var t = (UniFileScriptBehaviour)target;
        if (GUI.changed && t.script != null)
            t.scriptPath = AssetDatabase.GetAssetPath(t.script);
    }
}