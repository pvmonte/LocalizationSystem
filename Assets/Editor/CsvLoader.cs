using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CsvLoader
{
    string csvFilePath = "localization";
    char[] lineSeparators = new char[] { '\n'};
    char[] columnSeparators = new char[] { ',' };

    TextAsset csvFile;
    public List<string> header = new List<string>();
    public List<string[]> tableLines = new List<string[]>();

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
        string[] lines = csvFile.text.Split(lineSeparators);

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
