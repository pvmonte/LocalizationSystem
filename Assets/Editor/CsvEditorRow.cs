using System.Collections.Generic;

public class CsvEditorRow
{
    public List<string> elements;
    public bool isEditing;

    public CsvEditorRow(List<string> elements, bool isEditing)
    {
        this.elements = elements;
        this.isEditing = isEditing;
    }

    public string RowToString(bool withKey)
    {
        List<string> rowString = elements;

        if (!withKey)
            rowString.RemoveAt(0);

        return string.Join(",", rowString);
    }
}
