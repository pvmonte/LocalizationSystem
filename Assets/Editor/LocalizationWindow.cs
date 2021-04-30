using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LocalizationWindow : EditorWindow
{
    static CsvLoader loader;
    static List<CsvEditorRow> rows;

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

    bool isEditingRow;
    int editingLineIndex;

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
        loader = new CsvLoader();
        rows = new List<CsvEditorRow>();

        keysColumn = new List<string>();
        newKeysColumn = new List<string>();
        newRows = new List<CsvEditorRow>();

        for (int i = 0; i < loader.tableLines.Count; i++)
        {
            var line = loader.tableLines[i];
            List<string> list = new List<string>();
            list.AddRange(line);
            CsvEditorRow row = new CsvEditorRow(list, false);
            keysColumn.Add(line[0]);
            rows.Add(row);
        }
    }

    private void OnGUI()
    {
        float windowWidth = position.width;
        usableWidth = windowWidth - rowElementsSize - endLineButtonsSize * 3;

        if (loader == null)
            InitializeRows();

        BuildHeader();

        EditorGUILayout.BeginHorizontal();

        BuildKeysColumn();

        BuildTableContent();

        BuildButtonsColumn();

        EditorGUILayout.EndHorizontal();

        BuildFooterButtons();


    }

    public void BuildHeader()
    {
        EditorGUILayout.BeginHorizontal();
        var header = loader.header;

        for (int i = 0; i < header.Count; i++)
        {
            EditorGUILayout.LabelField(header[i], rowElementsWidth);
        }

        if(isAddingToHeader)
        {
            valueAdding = GUILayout.TextArea(valueAdding, GUILayout.Width(75));

            if (GUILayout.Button("Confirm", GUILayout.Width(75)))
            {
                CsvSaver saver = new CsvSaver(loader);
                saver.AddColumn(valueAdding);
                isAddingToHeader = false;
                valueAdding = string.Empty;

                Refresh();
            }
        }
        else
        {
            if (GUILayout.Button("+", endLineButtonsWidth))
            {
                isAddingToHeader = true;
            }
        }

        EditorGUILayout.EndHorizontal();
    }

    void BuildKeysColumn()
    {
        EditorGUILayout.BeginVertical(rowElementsWidth);

        for (int i = 0; i < keysColumn.Count; i++)
        {
            var row = rows[i];

            //TODO: Find a way to better this code
            if (row.isEditing)
            {
                keysColumn[i] = GUILayout.TextArea(keysColumn[i], rowElementsWidth);
            }
            else
            {
                EditorGUILayout.LabelField(keysColumn[i], rowElementsWidth);
            }
        }

        EditorGUILayout.EndVertical();
    }

    void BuildTableContent()
    {
        scrollPos = EditorGUILayout.BeginScrollView(Vector2.zero, GUILayout.Width(usableWidth));
        
        for (int i = 0; i < rows.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            var row = rows[i];

            for (int j = 1; j < row.elements.Count; j++)
            {
                DrawRowElement(j, row, row.elements);
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
    }

    private static void DrawRowElement(int i, CsvEditorRow row, List<string> elementsArray)
    {
        if (row.isEditing)
        {
            elementsArray[i] = GUILayout.TextArea(elementsArray[i], rowElementsWidth);
        }
        else
        {
            EditorGUILayout.LabelField(elementsArray[i], rowElementsWidth);
        }
    }

    void BuildButtonsColumn()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(endLineButtonsSize * 3));


        for (int i = 0; i < rows.Count; i++)
        {            
            EditorGUILayout.BeginHorizontal();

            if(GUILayout.Button("E", endLineButtonsWidth))
            {
                isEditingRow = true;
                rows[i].isEditing = true;
                editingLineIndex = i;
            }

            if (GUILayout.Button("-", endLineButtonsWidth))
            {
                CsvSaver saver = new CsvSaver(loader);
                saver.RemoveLine(i);
                rows.RemoveAt(i);
                keysColumn.RemoveAt(i);

                Refresh();
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();
    }

    void BuildFooterButtons()
    {
        if (isEditingRow)
        {
            if (GUILayout.Button("Confirm"))
            {
                //TODO
                CsvSaver saver = new CsvSaver(loader);
                string lineString = rows[editingLineIndex].RowToString(false);
                saver.EditLineByIndex(editingLineIndex, lineString);                
                isEditingRow = false;
            }

            if (GUILayout.Button("Cancel"))
            {
                rows[editingLineIndex].isEditing = false;
                editingLineIndex = -1;                
                isEditingRow = false;
            }
        }
        else
        {
            if (GUILayout.Button("+"))
            {
                //TODO
                List<string> elements = new List<string>();

                for (int i = 0; i < loader.header.Count; i++)
                {
                    elements.Add("");
                }

                var newRow = new CsvEditorRow(elements, true);
                newRow.elements[0] = "new-key";

                keysColumn.Add("new-key");
                newKeysColumn.Add("new-key");
                rows.Add(newRow);
                newRows.Add(newRow);
                Debug.Log("Ading");

            }

            if (GUILayout.Button("Save"))
            {
                //TODO
                for (int i = 0; i < newKeysColumn.Count; i++)
                {
                    newKeysColumn[i] = keysColumn[keysColumn.Count - 1];
                    newRows[i].elements[0] = newKeysColumn[i];

                    var rowElements = newRows[i].elements.ToArray();


                    Debug.Log(string.Join(",", rowElements));
                    CsvSaver saver = new CsvSaver(loader);
                    saver.AddLine(rowElements);
                }

                newKeysColumn.Clear();
                newRows.Clear();


                foreach (var item in rows)
                {
                    item.isEditing = false;
                }
                Debug.Log("Saved");
            }

            if (GUILayout.Button("Refresh"))
            {
                //TODO
                Refresh();
            }
        }
    }

    private static void Refresh()
    {
        AssetDatabase.Refresh();
        InitializeRows();
    }
}
