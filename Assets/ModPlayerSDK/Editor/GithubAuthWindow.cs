using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GithubAuthWindow : EditorWindow
{
    public Action<string> onReceiveAccessToken;
    private GithubWebView webView;

    private class GithubWebView : WebViewHook
    {
        public Action<string> onReceiveCode;

        protected override void OnLocationChanged(string url)
        {
            base.OnLocationChanged(url);

            if (url.Contains("?code=") == false) {
                this.ExecuteJavascript("setTimeout(function() {document.getElementById('js-oauth-authorize-btn').removeAttribute('disabled');}, 3000);");
                return;
            }

            var code = url.Split(new string[] { "?code=" },
                StringSplitOptions.None)[1];

            onReceiveCode?.Invoke(code);
        }
    }

    public static void ShowOAuthView(Action<string> onAccessToken)
    {
        var win = new GithubAuthWindow();
        win.onReceiveAccessToken = onAccessToken;
        win.minSize = win.maxSize = new Vector2(800, 800);
        win.Show();
    }

    void OnEnable()
    {
        if (!webView)
        {
            webView = CreateInstance<GithubWebView>();
            webView.onReceiveCode = OnReceiveCode;
            webView.ShowDevTools();
        }
    }
    public void OnBecameInvisible()
    {
        if (webView)
            webView.Detach();
    }

    void OnDestroy()
    {
        DestroyImmediate(webView);
    }

    void OnGUI()
    {
        if (webView.Hook(this))
            webView.LoadURL("https://github.com/login/oauth/authorize?client_id=3d626258bce9aba39399");

        webView.OnGUI(new Rect(Vector2.zero, position.size));
    }

    private void OnReceiveCode(string code)
    {
        var form = new WWWForm();   
        form.AddField("client_id", "3d626258bce9aba39399");
        form.AddField("client_secret", "ca3d2a2db41dd085b35b0490e4b56b5179e75422");
        form.AddField("code", code);
        var www = new WWW("https://github.com/login/oauth/access_token", form);

        while (www.isDone == false)
            ;

        if (string.IsNullOrEmpty(www.error) == false)
            Debug.LogError(www.error);

        var token = www.text.Split(new string[] { "access_token=" }, StringSplitOptions.None)[1];
        token = token.Split('&')[0];

        onReceiveAccessToken?.Invoke(token);

        Close();
    }
}
