using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TranslattionWindow : EditorWindow
{
    string myString = "Hello";
    static CsvLoader loader;

    [MenuItem("Window/Translation")]
    public static void ShowWindow()
    {
        GetWindow<TranslattionWindow>("Translation Manager");
        loader = new CsvLoader();
    }

    private void OnGUI()
    {
        if(loader == null)
            loader = new CsvLoader();

        EditorGUILayout.BeginVertical();        

        BuildHeader();       

        BuildAllLocalizationRows();

        EditorGUILayout.EndVertical();

        BuildFooterButtons();
        
    }

    public void BuildHeader()
    {
        EditorGUILayout.BeginHorizontal();
        var header = loader.tableLines[0];

        for (int i = 0; i < header.Length; i++)
        {
            GUILayout.Label(header[0]);
        }

        if (GUILayout.Button("+", GUILayout.Width(18)))
        {
            //TODO
            Debug.Log("Include Column");
        }

        EditorGUILayout.EndHorizontal();
    }

    public void BuildAllLocalizationRows()
    {
        for (int i = 1; i < loader.tableLines.Count; i++)
        {
            BuildLocalizationRow(i);
        }
    }

    public void BuildLocalizationRow(int line)
    {
        EditorGUILayout.BeginHorizontal();
        var tableLines = loader.tableLines;

        BuildLocalizationRowFields(tableLines[line]);
        for (int i = 1; i < tableLines.Count; i++)
        {
            
        }

        if (GUILayout.Button("-", GUILayout.Width(18)))
        {
            //TODO
            Debug.Log("Excluded");
        }

        EditorGUILayout.EndHorizontal();
    }

    public void BuildLocalizationRowFields(string[] line)
    {
        for (int i = 0; i < line.Length; i++)
        {
            EditorGUILayout.TextField(line[i]);            
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
            Debug.Log("Saved");
        }

        if (GUILayout.Button("Refresh"))
        {
            //TODO
            ShowWindow();
        }
    }
}
