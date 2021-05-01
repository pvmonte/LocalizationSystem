using UnityEngine;
using UnityEditor;

public class CellDrawer
{
    static float rowElementsSize = 150;
    static GUILayoutOption rowElementsWidth = GUILayout.Width(rowElementsSize);

    public void DrawLabel(string value)
    {
        EditorGUILayout.LabelField(value, rowElementsWidth);
    }

    public string DrawField(string value)
    {
        return EditorGUILayout.TextField(value, rowElementsWidth);
    }

    public void DrawEmpty(int width)
    {
        EditorGUILayout.LabelField("", GUILayout.Width(width));
    }
}
