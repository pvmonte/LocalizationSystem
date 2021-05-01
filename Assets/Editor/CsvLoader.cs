using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class CsvLoader
{
    public string csvResourcesPath { get; } = "localization";
    public char lineSeparator { get; } = '\n';
    public char[] fieldSeparators { get; } = new char[] { ',' };

    public TextAsset csvFile { get; private set; }
    public string[] lines { get; private set; }

    public string absolutePath { get; } = "Assets/Resources/localization.csv";

    public CsvLoader()
    {
        LoadCsv();
    }

    public void LoadCsv()
    {
        csvFile = Resources.Load<TextAsset>(csvResourcesPath);
        LinesAsArray();
    }

    void LinesAsArray()
    {
        lines = csvFile.text.Split(lineSeparator);
    }

    string[] SplitLineToValues(string line)
    {
        return line.Split(fieldSeparators);
    }

    public void AddLine(string line)
    {
        var key = line.Split(fieldSeparators)[0];

        string[] thisKey = lines.Where(x => x.Split(fieldSeparators)[0] == key).ToArray();
        if (thisKey != null)
        {
            Debug.LogError("Key already exits");
            return;
        }

        var apended = $"\n{line}";
        File.AppendAllText(absolutePath, apended);
        UnityEditor.AssetDatabase.Refresh();
    }

    public void Remove(string key)
    {
        string[] lines = csvFile.text.Split(lineSeparator);
        string[] keys = new string[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];

            keys[i] = line.Split(fieldSeparators, System.StringSplitOptions.None)[0];
        }

        int index = -1;

        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i].Contains(key))
            {
                index = i;
                break;
            }
        }

        if (index > -1)
        {
            string[] newLines;
            newLines = lines.Where(w => w != lines[index]).ToArray();

            string replaced = string.Join(lineSeparator.ToString(), newLines);
            File.WriteAllText(absolutePath, replaced);
            UnityEditor.AssetDatabase.Refresh();
        }
    }

    public void Edit(string line)
    {
        string key = line.Split(fieldSeparators)[0];

        string[] editingLine = lines.Where(x => x.Split(fieldSeparators)[0] == key).ToArray();

        Remove(key);
        AddLine(line);
        UnityEditor.AssetDatabase.Refresh();
    }

    public void AddColumn(string language)
    {
        string[] lines = csvFile.text.Split(lineSeparator);

        for (int i = 0; i < lines.Length; i++)
        {
            string newCell = "";

            if (i == 0)
                newCell = language;
            else
                newCell = "";

            lines[i] = string.Join(",", lines[i], newCell);
        }

        UnityEditor.AssetDatabase.Refresh();
    }
}
