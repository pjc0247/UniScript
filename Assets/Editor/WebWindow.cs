
using UnityEditor;
using UnityEngine;

class WebWindow : EditorWindow
{

    WebViewHook webView;
    string url = "https://google.com";

    [MenuItem("Tools/Web Window %#w")]
    static void Load()
    {
        WebWindow window = GetWindow<WebWindow>();
        window.Show();
    }

    void OnEnable()
    {
        if (!webView)
        {
            // create webView
            webView = CreateInstance<WebViewHook>();
        }
    }

    public void OnBecameInvisible()
    {
        if (webView)
        {
            // signal the browser to unhook
            webView.Detach();
        }
    }

    void OnDestroy()
    {
        //Destroy web view
        DestroyImmediate(webView);
    }

    void OnGUI()
    {
        // hook to this window
        if (webView.Hook(this))
            // do the first thing to do
            webView.LoadURL(url);

        // Navigation
        if (GUI.Button(new Rect(0, 0, 25, 20), "←"))
            webView.Back();
        if (GUI.Button(new Rect(25, 0, 25, 20), "→"))
            webView.Forward();

        // URL text field
        GUI.SetNextControlName("urlfield");
        url = GUI.TextField(new Rect(50, 0, position.width - 50, 20), url);
        var ev = Event.current;

        // Focus on web view if return is pressed in URL field
        if (ev.isKey && GUI.GetNameOfFocusedControl().Equals("urlfield"))
            if (ev.keyCode == KeyCode.Return)
            {
                webView.LoadURL(url);
                GUIUtility.keyboardControl = 0;
                webView.SetApplicationFocus(true);
                ev.Use();
            }
        //  else if (ev.keyCode == KeyCode.A && (ev.control | ev.command))


        if (ev.type == EventType.Repaint)
        {
            // keep the browser aware with resize
            webView.OnGUI(new Rect(0, 20, position.width, position.height - 20));
        }
    }
}
