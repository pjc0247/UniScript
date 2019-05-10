using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ModPlayerSDK
{
    public class ModPlayerWindow : EditorWindow
    {
        private Texture2D logo;

        protected virtual void Draw()
        {

        }

        public void OnGUI()
        {
            EnsureResources();

            var scale = (maxSize.x / 450.0f);
            var textureHeight = 100 * scale;
            GUI.DrawTexture(new Rect(Vector2.zero, minSize), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(0, 0, maxSize.x, textureHeight), logo);

            EditorGUILayout.BeginVertical();
            GUILayout.Space(textureHeight + 30 * scale);
            Draw();
            EditorGUILayout.EndVertical();
        }
        private void EnsureResources()
        {
            if (logo == null)
                logo = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/ModPlayerSDK/Images/modplayer.png");
        }
    }
}