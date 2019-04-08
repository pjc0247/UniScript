using UnityEditor;
using UnityEngine;

// So I want to communicate two-way between WebView and Editor
// But how? Socket Port?
// Jeez. I'm crazy. Let's do it!

class WebData : EditorWindow
{

    WebViewHook webView;

    public string data;

    WebSocketHook socket;

    WebSocketHook.Item hook;

    [MenuItem("Tools/Web Data Binding %#d")]
    static void Load()
    {
        WebData window = GetWindow<WebData>();
        window.Show();
    }

    void OnEnable()
    {
        if (!webView)
        {
            // Create webView
            webView = CreateInstance<WebViewHook>();
        }

        // Make the server.
        // The server is not serializable so there's
        // no need to check if this exist or not.
        socket = new WebSocketHook(9369, webView);

        // Hook window.data to javascript
        hook = socket.Add("data", () => data, (x) =>
        {
            data = x;
            EditorApplication.delayCall += () => Repaint();
        });
    }

    void OnDisable()
    {
        // IMPORTANT: Kill our server
        socket.Dispose();
    }

    public void OnBecameInvisible()
    {
        if (webView)
        {
            // signal the browser to unhook
            webView.Detach();
        }
    }

    // HTML for demonstration
    string Compose()
    {
        return @"<html><body>
    <textarea wrap='soft' rows=20 id='input'></textarea>
    <script>

        var i = document.getElementById('input');
        function update() { i.value = window.data; }
        i.oninput = function() { window.data = i.value };

    </script></body></html>";

    }

    void OnGUI()
    {
        // hook to this window
        if (webView.Hook(this))
        {
            // do the first thing to do
            webView.LoadHTML(Compose());
        }

        var half = new Rect(0, 0, position.width / 2, position.height);

        // html text field
        EditorGUI.BeginChangeCheck();
        data = EditorGUI.TextArea(half, data);
        if (EditorGUI.EndChangeCheck())
        {
            // push changes to webView and notify our input
            hook.Update();
            webView.ExecuteJavascript("update()");
        }

        half.x += half.width;

        if (Event.current.type == EventType.Repaint)
        {
            // keep the browser aware with resize
            webView.OnGUI(half);
        }
        else if (half.Contains(Event.current.mousePosition))
        {
            GUIUtility.keyboardControl = 0;
        }
    }
}
