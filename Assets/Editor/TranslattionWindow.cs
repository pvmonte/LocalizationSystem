using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TranslattionWindow : EditorWindow
{
    static CsvLoader loader;
    static List<CsvEditorRow> rows;

    //Size configurations
    float endLineButtonsSize = 18;
    float rowElementsSize = 50;
    private Vector2 scrollPos;

    [MenuItem("Window/Translation")]
    public static void ShowWindow()
    {
        GetWindow<TranslattionWindow>("Translation Manager");
        InitializeRows();
    }

    private static void InitializeRows()
    {
        loader = new CsvLoader();
        rows = new List<CsvEditorRow>();

        for (int i = 0; i < loader.tableLines.Count; i++)
        {
            List<string> list = new List<string>();
            list.AddRange(loader.tableLines[i]);

            CsvEditorRow row = new CsvEditorRow(list, false);
            rows.Add(row);
        }
    }
    
    private void OnGUI()
    {
        float windowWidth = position.width;
        float usableWidth = windowWidth - endLineButtonsSize * 3;
        rowElementsSize = 120;        

        if (loader == null)
            InitializeRows();

        //scrollPos = EditorGUILayout.BeginScrollView(Vector2.zero, GUILayout.Width(rowElementsSize));

        EditorGUILayout.BeginVertical();        

        BuildHeader();        

        BuildAllLocalizationRows();

        //EditorGUILayout.EndScrollView();

        EditorGUILayout.EndVertical();

        BuildFooterButtons();

        
        
    }

    public void BuildHeader()
    {
        EditorGUILayout.BeginHorizontal();
        var header = loader.tableLines[0];

        for (int i = 0; i < header.Length; i++)
        {
            EditorGUILayout.LabelField(header[i], GUILayout.Width(rowElementsSize));
        }

        if (GUILayout.Button("+", GUILayout.Width(endLineButtonsSize)))
        {
            //TODO
            Debug.Log("Include Column");
        }

        EditorGUILayout.EndHorizontal();
    }

    public void BuildAllLocalizationRows()
    {
        for (int i = 1; i < rows.Count; i++)
        {
            BuildLocalizationRow(i);
        }
    }

    public void BuildLocalizationRow(int line)
    {
        EditorGUILayout.BeginHorizontal();

        BuildLocalizationRowFields(rows[line]);

        if (GUILayout.Button("E", GUILayout.Width(endLineButtonsSize)))
        {
            //TODO
            rows[line].isEditing = true;
            Debug.Log("Editing");
        }

        if (GUILayout.Button("-", GUILayout.Width(endLineButtonsSize)))
        {
            //TODO
            Debug.Log("Excluded");
        }

        EditorGUILayout.EndHorizontal();
    }

    public void BuildLocalizationRowFields(CsvEditorRow row)
    {
        for (int i = 0; i < row.elements.Count; i++)
        {
            if (row.isEditing)
            {
                GUILayout.TextArea(row.elements[i], GUILayout.Width(rowElementsSize));                
            }
            else
            {
                EditorGUILayout.LabelField(row.elements[i], GUILayout.Width(rowElementsSize));
            }
        }
    }

    void BuildFooterButtons()
    {
        if (GUILayout.Button("+"))
        {
            //TODO
            Debug.Log("Ading");
        }

        if (GUILayout.Button("Save"))
        {
            //TODO
            foreach (var item in rows)
            {
                item.isEditing = false;
            }
            Debug.Log("Saved");
        }

        if (GUILayout.Button("Refresh"))
        {
            //TODO
            ShowWindow();
        }
    }
}
