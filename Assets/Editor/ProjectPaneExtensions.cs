using System.IO;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class ProjectPaneExtensions
{
    static ProjectPaneExtensions()
    {
        EditorApplication.projectWindowItemOnGUI -= Draw;
        EditorApplication.projectWindowItemOnGUI += Draw;
    }

    static GUIStyle _labelStyle;
    static void Draw(string guid, Rect selectionrect)
    {
        var path = AssetDatabase.GUIDToAssetPath(guid);

        // skip invalid names
        if (string.IsNullOrWhiteSpace(path))
            return;

        // don't touch the two-column layout
        if (selectionrect.height > 20)
            return;

        if (_labelStyle == null)
            _labelStyle = new GUIStyle(EditorStyles.label);

        var filename = Path.GetFileNameWithoutExtension(path);
        var extension = Path.GetExtension(path);

        var drawRect = selectionrect;
        //drawRect.x += _labelStyle.CalcSize(new GUIContent(filename)).x + 12;
        drawRect.x += _labelStyle.CalcSize(new GUIContent(filename)).x + 30;
        drawRect.y++;

        GUI.Label(drawRect, extension);
    }
}
