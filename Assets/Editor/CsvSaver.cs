using UnityEngine;
using System.IO;
using System.Linq;

public class CsvSaver
{
    CsvLoader loader;

    public CsvSaver(CsvLoader loader)
    {
        this.loader = loader;
    }

    public void AddColumn(string language)
    {
        Debug.Log("adding column");
        string[] lines = loader.csvFile.text.Split(loader.lineSeparator);
        string[] newLines = new string[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            Debug.Log("For " + i);
            
            if (i == 0)
            {
                lines[0] = string.Join(",", lines[0], language);
                newLines[0] = lines[0];
                Debug.Log(lines[0]);
            }            
            else
            {
                string[] lineAsArray = lines[i].Split(loader.columnSeparators);
                string[] newLineAsArray = new string[lineAsArray.Length + 1];

                for (int j = 0; j < lineAsArray.Length; j++)
                {
                    Debug.Log(j);
                    newLineAsArray[j] = lineAsArray[j];
                }

                newLineAsArray[newLineAsArray.Length - 1] = "";

                newLines[i] = string.Join(",", newLineAsArray);
                Debug.Log(string.Join("\n", newLines));
            }
            
        }
        
        string replaced = string.Join(",", newLines);
        File.WriteAllText(loader.absolutePath, replaced);
        
    }

    public void AddLine(params string[] texts)
    {
        if (string.IsNullOrEmpty(texts[0]))
        {
            throw new System.ArgumentException("key must have a value");
        }
                
        string appended = string.Join(",", texts);
        string appendedLine = $"\n{appended}";
        File.AppendAllText(loader.absolutePath, appendedLine);
        UnityEditor.AssetDatabase.Refresh();
    }

    public void RemoveLine(string key)
    {
        string[] lines = loader.csvFile.text.Split(loader.lineSeparator);
        string[] keys = ExtractCsvKeys(lines);
        int index = GetKeyIndex(key, keys);
        SaveNotRemovedLines(lines, index);
    }

    public void RemoveLine(int index)
    {
        string[] lines = loader.csvFile.text.Split(loader.lineSeparator);
        SaveNotRemovedLines(lines, index);
    }

    private void SaveNotRemovedLines(string[] lines, int index)
    {
        if (index > -1)
        {
            //Offset Index to assure it wond get the header
            index++;
            Debug.Log(index);
            string[] newLines;
            newLines = lines.Where(w => w != lines[index]).ToArray();

            foreach (var item in newLines)
            {
                Debug.Log(item[0]);
            }            

            string replaced = string.Join(loader.lineSeparator.ToString(), newLines);
            File.WriteAllText(loader.absolutePath, replaced);
        }
    }

    private static int GetKeyIndex(string key, string[] keys)
    {
        int index = -1;

        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i].Contains(key))
            {
                index = i;
                break;
            }
        }

        return index;
    }

    private string[] ExtractCsvKeys(string[] lines)
    {
        string[] keys = new string[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];

            keys[i] = line.Split(loader.columnSeparators, System.StringSplitOptions.None)[0];
        }

        return keys;
    }

    public void EditLine(string key, string value)
    {
        RemoveLine(key);
        AddLine(key, value);
    }

    public void EditLineByIndex(int index, string value)
    {
        
        string[] lines = loader.csvFile.text.Split(loader.lineSeparator);
        string[] keys = ExtractCsvKeys(lines);

        RemoveLine(index);

        //Offset index to ignore header
        index++;

        string key = keys[index];

        AddLine(key, value);
    }
}
