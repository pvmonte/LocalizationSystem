using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsvLoader
{
    public string csvFilePath { get; } = "localization";
    public char lineSeparator { get; } = '\n';
    public char[] columnSeparators { get; } = new char[] { ',' };

    public TextAsset csvFile { get; private set; }
    public List<string> header = new List<string>();
    public List<string[]> tableLines = new List<string[]>();

    public string absolutePath { get; } = "Assets/Resources/localization.csv";

    public CsvLoader()
    {
        CsvLineToTableLineArray();
    }

    public void LoadCsv()
    {
        csvFile = Resources.Load<TextAsset>(csvFilePath);
    }

    public void CsvLineToTableLineArray()
    {
        LoadCsv();
        string[] lines = csvFile.text.Split(lineSeparator);

        var headerLine = SplitLineToValues(lines[0]);
        header.AddRange(headerLine); 

        for (int i = 1; i < lines.Length; i++)
        {
            var tableLine = SplitLineToValues(lines[i]);
            tableLines.Add(tableLine);            
        }
    }

    string[] SplitLineToValues(string line)
    {
        return line.Split(columnSeparators);
    }
}
