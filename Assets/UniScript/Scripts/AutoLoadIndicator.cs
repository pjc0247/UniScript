using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AutoLoadIndicator : MonoBehaviour
{
    public static bool applied = false;

#if UNITY_EDITOR
    private Texture2D cssTex;

    private static bool showModified = false;
    private static DateTime showModifiedSince;

    private float alpha = 0;

    private void drawString(string text, Color? colour = null, bool showTexture = true, int offsetY = 0)
    {
        Handles.BeginGUI();

        var restoreColor = GUI.color;
        var viewSize = SceneView.currentDrawingSceneView.position;

        if (colour.HasValue) GUI.color = colour.Value;
        var view = SceneView.currentDrawingSceneView;
        Vector3 screenPos = view.camera.WorldToScreenPoint(
            view.camera.transform.position + view.camera.transform.forward);

        if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.z < 0)
        {
            GUI.color = restoreColor;
            Handles.EndGUI();
            return;
        }
        var style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = 50;
        Vector2 size = style.CalcSize(new GUIContent(text));
        GUI.Label(new Rect(40, viewSize.height - 100 - offsetY, size.x, size.y), text, style);
        GUI.color = restoreColor;

        if (showTexture)
        {
            if (cssTex == null)
                OnEnable();
            GUI.DrawTexture(
                new Rect(40, viewSize.height - 220, 100, 100), cssTex,
                ScaleMode.StretchToFill, true, 1.0f, new Color(1, 1, 1, alpha), 0, 0);
        }

        Handles.EndGUI();
    }

    void OnEnable()
    {
        cssTex = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/USS/Resources/css.png");
    }
    void OnDrawGizmos()
    {
        if (applied)
        {
            applied = false;

            showModified = true;
            showModifiedSince = DateTime.Now;
        }

        if (showModified)
        {
            var delta = (DateTime.Now - showModifiedSince);
            if (delta <= TimeSpan.FromSeconds(1.5f))
            {
                alpha = 1.0f - (float)delta.TotalSeconds / 1.5f;

                drawString(".csx reloaded",
                    new Color(1, 0, 0, alpha),
                    true, 0);
            }
            else
                showModified = false;
        }

        /*
        if (UssStyleModifier.hasError)
        {
            alpha = 1;
            drawString("Your .ucss has parsing error(s).\r\n" +
                "Please check console for details",
                Color.red,
                false, 70);
        }
        */
    }
#endif
}