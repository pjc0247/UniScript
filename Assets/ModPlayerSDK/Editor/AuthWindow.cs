using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Firebase.Auth;

public class AuthWindow : EditorWindow
{
    private Texture2D logo;

    [MenuItem("ModPlayer/Login")]
    public static void ShowAuthWindow()
    {
        var win = new AuthWindow();
        win.minSize = win.maxSize = new Vector2(450, 240);
        win.titleContent = new GUIContent(
            "Login to ModPlayer",
            AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/ModPlayerSDK/Images/icon.png"));
        win.Show();
    }
    [MenuItem("ModPlayer/Login", validate = true)]
    private static bool LoginCond()
    {
        return ModPlayerFB.Auth.CurrentUser == null;
    }
    [MenuItem("ModPlayer/Logout")]
    public static void Logout()
    {
        var auth = ModPlayerFB.Auth;
        if (auth.CurrentUser != null)
            auth.SignOut();
    }
    [MenuItem("ModPlayer/Logout", validate = true)]
    private static bool LogoutCond()
    {
        return ModPlayerFB.Auth.CurrentUser != null;
    }

    public void OnGUI()
    {
        EnsureResources();

        GUI.DrawTexture(new Rect(Vector2.zero, minSize), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(0, 0, 450, 100), logo);

        EditorGUILayout.BeginVertical();
        GUILayout.Space(130);

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("Github", GUILayout.Width(200), GUILayout.Height(35)))
        {
            GithubAuthWindow.ShowOAuthView(LoginGithub);
        }
        if (GUILayout.Button("Google", GUILayout.Width(200), GUILayout.Height(35)))
        {
            GoogleAuthWindow.ShowOAuthView(LoginGoogle);
        }
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }
    private void EnsureResources()
    {
        if (logo == null)
            logo = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/ModPlayerSDK/Images/modplayer.png");
    }

    private void LoginGoogle(string accessToken)
    {
        Debug.Log("[LoginGoogle] " + accessToken);

        var auth = ModPlayerFB.Auth;
        var credential =
            GoogleAuthProvider.GetCredential("", accessToken);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            var newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

            Close();
        });
    }
    private void LoginGithub(string accessToken)
    {
        Debug.Log("[LoginGithub] " + accessToken);

        var auth = ModPlayerFB.Auth;
        var credential =
            GitHubAuthProvider.GetCredential(accessToken);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            var newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

            Close();
        });
    }
}
