using UnityEditor;
using UnityEngine;

public class LineDrawer
{
    CellDrawer cellDrawer;
    public bool isEditing { get; set; }

    public LineDrawer(CellDrawer cellDrawer)
    {
        this.cellDrawer = cellDrawer;
    }

    public void Draw(string line)
    {
        string[] cells = line.Split(',');

        EditorGUILayout.BeginHorizontal();

        for (int i = 0; i < cells.Length; i++)
        {
            cellDrawer.DrawLabel(cells[i]);
        }

        EditorGUILayout.EndHorizontal();
    }

    public void DrawEditingLine(int cells, string[] values)
    {
        EditorGUILayout.BeginHorizontal();

        for (int i = 0; i < cells; i++)
        {
            values[i] = cellDrawer.DrawField(values[i]);
        }

        EditorGUILayout.EndHorizontal();
    }
}
