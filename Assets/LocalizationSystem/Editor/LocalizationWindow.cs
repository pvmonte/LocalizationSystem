using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LocalizationWindow : EditorWindow
{
    public static LocalizationWindow thisWindow;

    public CsvLoader csvLoader;

    IState state;

    //Size configurations
    static float endLineButtonsSize = 18;
    static float usableWidth;
    private Vector2 scrollPos;

    [MenuItem("Localization/Open Window")]
    public static void ShowWindow()
    {
        thisWindow = GetWindow<LocalizationWindow>("Localization Manager");
    }

    public void InitializeWindow()
    {
        if (thisWindow == null)
            thisWindow = GetWindow<LocalizationWindow>("Localization Manager");

        if (state == null)
            state = new StandardState(thisWindow);
    }

    private void InitializeRows()
    {
        csvLoader = new CsvLoader();
    }

    public void ChangeToState(IState newState)
    {
        state = newState;
    }

    private void OnGUI()
    {
        float windowWidth = position.width;
        usableWidth = windowWidth - endLineButtonsSize * 3;

        InitializeWindow();

        if (csvLoader == null)
            InitializeRows();

        EditorGUILayout.BeginHorizontal();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(usableWidth));

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
        state.DrawTable(lines);
    }

    void BuildButtonsColumn()
    {
        if (state.GetType() != typeof(StandardState))
            return;

        string[] lines = csvLoader.lines;
        state.DrawEditingColumnButtons(lines);
    }

    void BuildFooterButtons()
    {
        state.DrawFooter();
    }

    public void Refresh()
    {
        AssetDatabase.Refresh();
        InitializeRows();
    }
}

public class AddingColumnState : State, IState
{
    string newLanguage = "";

    public AddingColumnState(LocalizationWindow window) : base(window)
    {
    }

    public void DrawTable(string[] lines)
    {
        for (int i = 1; i < lines.Length; i++)
        {
            var lineDrawer = new LineDrawer(new CellDrawer());
            lineDrawer.Draw(lines[i]);
        }
    }

    public void DrawEditingColumnButtons(string[] lines)
    {
        throw new System.NotImplementedException();
    }

    public void DrawFooter()
    {
        EditorGUILayout.LabelField("Language: ");
        newLanguage = EditorGUILayout.TextField(newLanguage);

        if(GUILayout.Button("Add"))
        {
            window.csvLoader.AddColumn(newLanguage);
            window.ChangeToState(new StandardState(window));
            window.Refresh();
        }

        
    }
}
