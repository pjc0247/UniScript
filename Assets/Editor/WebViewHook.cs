/* WebHook 0.7 - https://github.com/willnode/WebViewHook/ - MIT */

using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class WebViewHook : ScriptableObject
{
    
    Object webView;
    EditorWindow host;
    object hostCache;

    static Type _T;
    static FieldInfo _Parent;
    static MethodInfo _Show, _Hide, _Back, _Reload, _Forward;
    static MethodInfo _SetSizeAndPosition;
    static MethodInfo _InitWebView;
    static MethodInfo _SetDelegateObject;
    static MethodInfo _AllowRightClickMenu;
    static MethodInfo _ShowDevTools;
    static MethodInfo _DefineScriptObject;
    static MethodInfo _SetHostView;
    static MethodInfo _ExecuteJavascript;
    static MethodInfo _LoadURL;
    static MethodInfo _HasApplicationFocus;
    static MethodInfo _SetApplicationFocus;
    static Func<Rect, Rect> _Unclip;

    static WebViewHook()
    {
        _T = typeof(Editor).Assembly.GetTypes().First(x => x.Name == "WebView");
        _Parent = typeof(EditorWindow).GetField("m_Parent", Instance);
        _Show = (_T.GetMethod("Show", Instance));
        _Hide = (_T.GetMethod("Hide", Instance));
        _Back = (_T.GetMethod("Back", Instance));
        _Reload = (_T.GetMethod("Reload", Instance));
        _Forward = (_T.GetMethod("Forward", Instance));
        _InitWebView = (_T.GetMethod("InitWebView", Instance));
        _SetSizeAndPosition = (_T.GetMethod("SetSizeAndPosition", Instance));
        _SetHostView = (_T.GetMethod("SetHostView", Instance));
        _AllowRightClickMenu = (_T.GetMethod("AllowRightClickMenu", Instance));
        _SetDelegateObject = (_T.GetMethod("SetDelegateObject", Instance));
        _ShowDevTools = (_T.GetMethod("ShowDevTools", Instance));
        _DefineScriptObject = (_T.GetMethod("DefineScriptObject", Instance));
        _ExecuteJavascript = (_T.GetMethod("ExecuteJavascript", Instance));
        _LoadURL = (_T.GetMethod("LoadURL", Instance));
        _HasApplicationFocus = (_T.GetMethod("HasApplicationFocus", Instance));
        _SetApplicationFocus = (_T.GetMethod("SetApplicationFocus", Instance));
        _Unclip = (Func<Rect, Rect>)Delegate.CreateDelegate(typeof(Func<Rect, Rect>), typeof(GUI).Assembly.GetTypes()
            .First(x => x.Name == "GUIClip").GetMethod("Unclip", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(Rect) }, null));

    }

    void OnEnable()
    {
        if (!webView)
        {
            webView = CreateInstance(_T);
            webView.hideFlags = HideFlags.DontSave;
        }

        // This is necessary because WebView messes whole editor if leaves open during assembly reload
        // Unfortunately this AssemblyReloadEvents only available in 2017.1 so we can't use WebView in 5.x
        // Will looking for another option if found.
        AssemblyReloadEvents.beforeAssemblyReload += OnDisable;
    }

    void OnDisable()
    {
        if (webView)
        {
            Detach();
        }

        AssemblyReloadEvents.beforeAssemblyReload -= OnDisable;
    }

    void OnDestroy()
    {
        DestroyImmediate(webView);
        webView = null;
    }

    public bool Hook(EditorWindow host)
    {
        if (host == this.host) return false;

        if (!webView)
            OnEnable();

        // initialization go here

        Invoke(_InitWebView, _Parent.GetValue(hostCache = (this.host = host)), 0, 0, 1, 1, false);
        Invoke(_SetDelegateObject, this);
        Invoke(_AllowRightClickMenu, true);

        return true;
    }

    public void Detach()
    {
        Invoke(_SetHostView, this.hostCache = null);
    }

    void SetHostView(object host)
    {
        Invoke(_SetHostView, this.hostCache = host);
        Hide();
        Show();
    }

    void SetSizeAndPosition(Rect position)
    {
        Invoke(_SetSizeAndPosition, (int)position.x, (int)position.y, (int)position.width, (int)position.height);
    }

    void OnGUI() { }

    public void OnGUI(Rect r)
    {
        if (host)
        {
            var h = _Parent.GetValue(host);
            if (hostCache != h)
                SetHostView(h);
            else
                Invoke(_SetHostView, h);
        }

        SetSizeAndPosition(_Unclip(r));
    }

    public void AllowRightClickMenu(bool yes)
    {
        Invoke(_AllowRightClickMenu, yes);
    }

    public void Forward()
    {
        Invoke(_Forward);
    }

    public void Back()
    {
        Invoke(_Back);
    }

    public void Show()
    {
        Invoke(_Show);
    }

    public void Hide()
    {
        Invoke(_Hide);
    }

    public void Reload()
    {
        Invoke(_Reload);
    }

    public bool HasApplicationFocus()
    {
        return (bool)_HasApplicationFocus.Invoke(webView, null);
    }

    public void SetApplicationFocus(bool focus)
    {
        Invoke(_SetApplicationFocus, focus);
    }

    protected void ShowDevTools()
    {
        // This method may not work
        Invoke(_ShowDevTools);
    }

    public void LoadURL(string url)
    {
        Invoke(_LoadURL, url);
    }

    public void LoadHTML(string html)
    {
        Invoke(_LoadURL, "data:text/html;charset=utf-8," + html);
    }

    public void LoadFile(string path)
    {
        Invoke(_LoadURL, "file:///" + path);
    }

    protected void DefineScriptObject(string path, ScriptableObject obj)
    {
        // This method has unknown behavior
        Invoke(_DefineScriptObject, path, obj);
    }

    protected void SetDelegateObject(ScriptableObject obj)
    {
        // Only set into this object
        Invoke(_SetDelegateObject, obj);
    }

    public void ExecuteJavascript(string js)
    {
        Invoke(_ExecuteJavascript, js);
    }

    void Invoke(MethodInfo m, params object[] args)
    {
        try
        {
            m.Invoke(webView, args);
        }
        catch (Exception) { }
    }

    const BindingFlags Instance = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy;

    /* Default bindings for SetDelegateObject */

    public Action<string> LoadError;

    public Action InitScripting;

    public Action<string> LocationChanged;

    protected virtual void OnLocationChanged(string url)
    {
        if (LocationChanged != null)
            LocationChanged(url);
    }

    protected virtual void OnLoadError(string url)
    {
        if (LoadError != null)
            LoadError(url);
        else
            Debug.LogError("WebView has failed to load " + url);
    }

    protected virtual void OnInitScripting()
    {
        if (InitScripting != null)
            InitScripting();
    }

    protected virtual void OnOpenExternalLink(string url)
    {
        // This binding may not work
    }

    protected virtual void OnWebViewDirty()
    {
        // This binding may not work
    }

    protected virtual void OnDownloadProgress(string id, string message, ulong bytes, ulong total)
    {
        // This binding may not work
    }

    protected virtual void OnBatchMode()
    {
        // This binding may not work
    }

    protected virtual void OnReceiveTitle(string title)
    {
        // This binding may not work
    }

    protected virtual void OnDomainReload()
    {
        // This binding may not work
    }

}
