using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEditor;

class CodeHighlighter
{
    public static void HighlightAndPrint(string source)
    {
        if (source == null)
            source = "";

        var keywords = new List<string>()
        {
            "static", "public", "private", "protected", "override", "class",
            "void", "int", "string", "float", "double", "var", "bool",
            "true", "false", "null",
            "new", "if ", "else", "try","catch", "finally", "return", "in",
            "switch", "case", "break"
        };

        GUIStyle style = EditorStyles.textArea;
        style.richText = true;

        var highlighted = source;

        // STRING
        var regex = new Regex("\"(.*)\"");
        highlighted = regex.Replace(highlighted, "<color=brown>\"$1\"</color>");

        // SINGLE LINE COMMENT
        regex = new Regex("\\/\\/(.*)$", RegexOptions.Multiline);
        highlighted = regex.Replace(highlighted, "<i><color=green>//$1</color></i>");

        // KEYWORD
        foreach (var kw in keywords)
        {
            highlighted = highlighted.Replace(kw, "<b><color=blue>" + kw + "</color></b>");
        }

        EditorGUILayout.TextArea(highlighted, style, GUILayout.Height(500));
    }
}