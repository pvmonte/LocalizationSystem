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

    Dictionary<string, Dictionary<string, string>> languageDictionaryPair { get; } = new Dictionary<string, Dictionary<string, string>>();
    public string absolutePath { get; } = "Assets/LocalizationSystem/Resources/localization.csv";

    public CsvLoader()
    {
        LoadCsv();
    }

    public void LoadCsv()
    {
        csvFile = Resources.Load<TextAsset>(csvResourcesPath);
        LinesAsArray();
        InitializeLanguageDictionaries();
    }

    public string GetLanguageDictionaryPairValue(string lang, string key)
    {
        return languageDictionaryPair[lang][key];
    }

    private void InitializeLanguageDictionaries()
    {
        var languages = lines[0].Split(fieldSeparators);

        for (int i = 0; i < languages.Length; i++)
        {
            Dictionary<string, string> keyValuePairs = GetDictionary(languages[i]);
            languageDictionaryPair.Add(languages[i], keyValuePairs);
        }
    }

    void LinesAsArray()
    {
        lines = csvFile.text.Split(lineSeparator);
    }

    Dictionary<string, string> GetDictionary(string language)
    {
        Dictionary<string, string> collection = new Dictionary<string, string>();
        int languageIndex = -1;

        string[] languages = lines[0].Split(fieldSeparators);

        //starting at 1 to ignore keys
        for (int i = 0; i < languages.Length; i++)
        {
            if (language == languages[i])
            {
                languageIndex = i;
                break;
            }
        }

        //Starting from 1 to ignore the header
        for (int i = 1; i < lines.Length; i++)
        {
            var line = lines[i].Split(fieldSeparators);
            collection.Add(line[0], line[languageIndex]);
        }

        return collection;
    }

    public string GetLocalizedText(string key, string language)
    {
        int languageIndex = -1;

        string[] languages = lines[0].Split(fieldSeparators);
        string[] keys = ExtractFileKeys(lines);

        //starting at 1 to ignore keys
        for (int i = 1; i < languages.Length; i++)
        {
            if (language == languages[i])
            {
                languageIndex = i;
                break;
            }                
        }

        if (languageIndex == -1)
            return string.Empty;
        
        var matchingLines = lines.Where(x => x.StartsWith(keys[languageIndex])).ToList();
        var lineAsArray = matchingLines[0].Split(fieldSeparators);
        return lineAsArray[languageIndex];
    }

    private string[] ExtractFileKeys(string[] lines)
    {
        string[] keys = new string[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];

            keys[i] = line.Split(fieldSeparators, System.StringSplitOptions.None)[0];
        }

        return keys;
    }

#if UNITY_EDITOR
    public void AddLine(string line)
    {
        var key = line.Split(fieldSeparators)[0];
        Debug.Log($"key {key}");

        //Search if there is matching keys
        string[] matchingKeys = lines.Where(x => x.Split(fieldSeparators)[0] == key).ToArray();
        Debug.Log($"matchingKeys length {matchingKeys.Length}");

        if (matchingKeys.Length > 0)
        {
            Debug.LogError("Key already exits");
            return;
        }

        AddLineToEnd(line);
    }

    private void AddLineToEnd(string line)
    {
        var apended = $"\n{line}";
        File.AppendAllText(absolutePath, apended);
        UnityEditor.AssetDatabase.Refresh();
    }

    public void Remove(string key)
    {
        string[] lines = csvFile.text.Split(lineSeparator);
        string[] keys = ExtractFileKeys(lines);

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

    public void Edit(int lineIndex, string newLine)
    {
        lines[lineIndex] = newLine;

        string allText = string.Join(lineSeparator.ToString(), lines);
        File.WriteAllText(absolutePath, allText);
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

        string text = string.Join(lineSeparator.ToString(), lines);
        File.WriteAllText(absolutePath, text);

        UnityEditor.AssetDatabase.Refresh();
    }
#endif
}
