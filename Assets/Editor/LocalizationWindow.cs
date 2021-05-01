using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LocalizationWindow : EditorWindow
{
    static CsvLoader csvLoader;
    static List<CsvEditorRow> rows;

    static bool isAdding;
    static string[] newLine;

    bool isEditingLine;
    int editingLineIndex;

    //Size configurations
    static float endLineButtonsSize = 18;
    static float rowElementsSize = 150;
    static float usableWidth;
    private Vector2 scrollPos;

    static GUILayoutOption endLineButtonsWidth = GUILayout.Width(endLineButtonsSize);
    static GUILayoutOption rowElementsWidth = GUILayout.Width(rowElementsSize);


    static List<string> keysColumn = new List<string>();

    bool isAddingToHeader;
    string valueAdding;



    //Add Parametter
    static List<string> newKeysColumn = new List<string>();
    static List<CsvEditorRow> newRows = new List<CsvEditorRow>();

    [MenuItem("Window/Localization")]
    public static void ShowWindow()
    {
        GetWindow<LocalizationWindow>("Localization Manager");
        InitializeRows();
    }

    private static void InitializeRows()
    {
        csvLoader = new CsvLoader();
        rows = new List<CsvEditorRow>();

        keysColumn = new List<string>();
        newKeysColumn = new List<string>();
        newRows = new List<CsvEditorRow>();
        /*
        for (int i = 0; i < csvLoader.tableLines.Count; i++)
        {
            var line = csvLoader.tableLines[i];
            List<string> list = new List<string>();
            list.AddRange(line);
            CsvEditorRow row = new CsvEditorRow(list, false);
            keysColumn.Add(line[0]);
            rows.Add(row);
        }
        */
    }

    private void OnGUI()
    {
        float windowWidth = position.width;
        usableWidth = windowWidth - endLineButtonsSize * 3;

        if (csvLoader == null)
            InitializeRows();

        EditorGUILayout.BeginHorizontal();

        scrollPos = EditorGUILayout.BeginScrollView(Vector2.zero, GUILayout.Width(usableWidth));

        EditorGUILayout.BeginVertical();

        BuildHeader();

        BuildTableContent();

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndScrollView();

        BuildButtonsColumn();

        EditorGUILayout.EndHorizontal();

        BuildFooterButtons();
    }

    public void BuildHeader()
    {
        EditorGUILayout.BeginHorizontal();

        string[] lines = csvLoader.lines;
        var header = lines[0];

        var lineDrawer = new LineDrawer(new CellDrawer());
        lineDrawer.Draw(header);

        EditorGUILayout.EndHorizontal();
    }

    void BuildTableContent()
    {
        string[] lines = csvLoader.lines;

        for (int i = 1; i < lines.Length; i++)
        {
            if (editingLineIndex == i)
            {
                LineEditing(lines[editingLineIndex]);
            }
            else
            {
                var lineDrawer = new LineDrawer(new CellDrawer());
                lineDrawer.Draw(lines[i]);
            }
        }

        if (isAdding)
        {
            LineEditing(lines[0]);
        }
    }

    private static void LineEditing(string line)
    {
        if (newLine == null)
            newLine = line.Split(',');

        var lineDrawer = new LineDrawer(new CellDrawer());
        lineDrawer.DrawEditingLine(newLine.Length, newLine);
    }

    void BuildButtonsColumn()
    {
        if (isAdding && isEditingLine)
            return;

        EditorGUILayout.BeginVertical(GUILayout.Width(endLineButtonsSize * 2));

        string[] lines = csvLoader.lines;

        for (int i = 0; i < lines.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();

            if (i == 0)
            {
                var cell = new CellDrawer();
                cell.DrawEmpty(20);
            }
            else
            {
                var lineButtonDrawer = new LineButtonDrawer();
                lineButtonDrawer.Draw("E", () =>
                {
                    isEditingLine = true;
                    editingLineIndex = i;
                });

                string[] keys = new string[lines.Length];
                string key = lines[i].Split(',')[0];

                lineButtonDrawer.Draw("-", () => {
                    csvLoader.Remove(key);
                    Refresh();
                });
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();
    }

    void BuildFooterButtons()
    {
        if (isEditingLine || isAdding)
        {
            DrawEditingButtons();
        }
        else
        {
            DrawNormalfooterButtons();
        }
    }

    private void DrawEditingButtons()
    {
        var footerButton = new FooterButtonDrawer();

        if(isAdding)
        {
            footerButton.Draw("Save", () =>
            {
                string addingLine = string.Join(",", newLine);
                csvLoader.AddLine(addingLine);
                EndEditing();
                Refresh();
            });
        }
        else if(isEditingLine)
        {
            footerButton.Draw("Edit", () =>
            {
                string editingLine = string.Join(",", newLine);
                csvLoader.Edit(editingLine);
                EndEditing();
                Refresh();
            });
        }

        footerButton.Draw("Cancel", () =>
        {
            EndEditing();
        });
    }

    private static void DrawNormalfooterButtons()
    {
        var footerButton = new FooterButtonDrawer();
        footerButton.Draw("+", () => isAdding = true);
        footerButton.Draw("Refresh", Refresh);
    }

    private void EndEditing()
    {
        newLine = null;
        isAdding = false;
        isEditingLine = false;
        editingLineIndex = -1;
    }

    private static void Refresh()
    {
        AssetDatabase.Refresh();
        InitializeRows();
    }
}
