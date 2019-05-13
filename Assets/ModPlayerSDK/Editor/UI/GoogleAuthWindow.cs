using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GoogleAuthWindow : EditorWindow
{
    public Action<string> onReceiveAccessToken;
    private GoogleWebView webView;

    private class GoogleWebView : WebViewHook
    {
        public Action<string> onReceiveCode;

        protected override void OnLocationChanged(string url)
        {
            base.OnLocationChanged(url);

            if (url.Contains("?code=") == false)
                return;

            var code = url.Split(new string[] { "?code=" },
                StringSplitOptions.None)[1];
            code = code.Split('&')[0];
            code = code.Replace("%2", "/");

            Debug.Log(url);
            Debug.Log(code);

            onReceiveCode?.Invoke(code);
        }
    }

    public static void ShowOAuthView(Action<string> onAccessToken)
    {
        var win = new GoogleAuthWindow();
        win.onReceiveAccessToken = onAccessToken;
        win.minSize = win.maxSize = new Vector2(800, 800);
        win.Show();
    }

    void OnEnable()
    {
        if (!webView)
        {
            webView = CreateInstance<GoogleWebView>();
            webView.onReceiveCode = OnReceiveCode;
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
            webView.LoadURL("https://accounts.google.com/o/oauth2/v2/auth?client_id=439172685345-ocdo6g9672rtsbumvm75j5mafpkev4p0.apps.googleusercontent.com&scope=profile&redirect_uri=https://modplayer-kr.firebaseapp.com/__/auth/handler&response_type=code&include_granted_scopes=true");

        webView.OnGUI(new Rect(Vector2.zero, position.size));
    }

    private void OnReceiveCode(string code)
    {
        var form = new WWWForm();
        form.AddField("client_id", "439172685345-ocdo6g9672rtsbumvm75j5mafpkev4p0.apps.googleusercontent.com");
        form.AddField("client_secret", "VdJhJYsGEwhncAhkaLjMW5_H");
        form.AddField("code", code);
        form.AddField("redirect_url", "https://modplayer-kr.firebaseapp.com/__/auth/handler");
        form.AddField("grant_type", "authorization_code");
        var www = new WWW("https://www.googleapis.com/oauth2/v4/token", form);

        while (www.isDone == false)
            ;

        if (string.IsNullOrEmpty(www.error) == false)
            Debug.LogError(www.error);

        Debug.Log(www.text);

        var token = www.text.Split(new string[] { "access_token=" }, StringSplitOptions.None)[1];
        token = token.Split('&')[0];

        onReceiveAccessToken?.Invoke(token);

        Close();
    }
}
