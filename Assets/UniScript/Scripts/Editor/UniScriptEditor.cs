using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// [CustomEditor(typeof(UniScriptBehaviour), true)]
// public class UniScriptEditor : Editor
// {
//     public string src = "";
//
//     public override void OnInspectorGUI()
//     {
//         base.OnInspectorGUI();
//
//         var unis = (UniScriptBehaviour)target;
//
//         CodeHighlighter.HighlightAndPrint(unis.src);
//         //src = EditorGUILayout.TextArea(src);
//         if (GUILayout.Button("Apply"))
//         {
//             unis.Bind(src);
//         }
//     }
// }
