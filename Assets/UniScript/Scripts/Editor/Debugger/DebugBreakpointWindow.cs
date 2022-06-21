using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DebugBreakpointWindow : EditorWindow
{
    [InitializeOnLoadMethod]
    static void Init()
    {
        RuntimeScript.showDebugger = (src, line) => {
            ShowWindow(src, line);
        };
    }

    [MenuItem("UniScript/Debugger")]
    public static void ShowWindow(string src, int line)
    {
        var window = new DebugBreakpointWindow();
        window.src = src;
        window.line = line;
        window.maxSize = new Vector2(490, 400);
        window.minSize = new Vector2(490, 400);
        window.titleContent = new GUIContent(
            "Breakpoint");
        window.Show();
    }

    private string src = "";
    private int line = 0;
    // private DebugWebviewHook webView;

    private string eval = "";
    private string evalResult = "";

    private Vector2 callstackScroll;

    void OnEnable()
    {
        // if (!webView)
        // {
        //     // create webView
        //     webView = CreateInstance<DebugWebviewHook>();
        // }
    }

    public void OnBecameInvisible()
    {
        // if (webView)
        // {
        //     // signal the browser to unhook
        //     webView.Detach();
        // }
    }

    void OnDestroy()
    {
        //Destroy web view
        // DestroyImmediate(webView);
    }

    void OnGUI()
    {
        if (Application.isPlaying == false)
        {
            Close();
            return;
        }

        // if (webView.Hook(this))
        // {
        //     webView.LoadURL("file:///" + Application.dataPath + "/UniScript/Scripts/Editor/Debugger/www/index.html");
        //     timer = 0.525f;
        // }
        //
        // webView.OnGUI(new Rect(Vector2.zero, new Vector2(490, 200)));

        var e = Event.current;

        EditorGUILayout.BeginVertical();
        GUILayout.Space(205);
        eval = EditorGUILayout.TextField("REPL", eval);
        if (e.keyCode == KeyCode.Return)
        {
            evalResult = RuntimeScript.Eval(eval);
        }
        EditorGUILayout.LabelField(evalResult);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(20);

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Locals", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        foreach (var local in RuntimeScript.dump.locals)
        {
            EditorGUILayout.TextField(local.Key, local.Value.ToString());
        }
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();

        GUILayout.Space(40);

        EditorGUILayout.BeginVertical();
        callstackScroll = EditorGUILayout.BeginScrollView(callstackScroll, true, false);
        EditorGUILayout.LabelField("Callstack", EditorStyles.boldLabel);
        foreach (var callFrame in RuntimeScript.dump.callStack.Reverse())
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField(callFrame.signature);
        }
        EditorGUI.indentLevel = 0;
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndFadeGroup();

        EditorGUILayout.EndVertical();
    }

    private float timer = 0;
    void Update()
    {
//         if (timer > 0)
//         {
//             timer -= Time.deltaTime;
//             if (timer <= 0)
//             {
//                 webView.ExecuteJavascript(@"
// setSrc(""" + src + @"""); ");
//                 webView.ExecuteJavascript(@"
// markLine(" + line + @"); ");
//             }
//         }
    }
}

// public class DebugWebviewHook : WebViewHook
// {
//     protected override void OnLocationChanged(string url)
//     {
//     }
//     protected override void OnInitScripting()
//     {
//     }
// }