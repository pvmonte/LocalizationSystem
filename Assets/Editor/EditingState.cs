public class EditingState : State
{
    protected string[] newLine;

    public EditingState(LocalizationWindow window) : base(window)
    {
    }

    protected void LineEditing(string line)
    {
        if (newLine == null)
            newLine = line.Split(',');

        var lineDrawer = new LineDrawer(new CellDrawer());
        lineDrawer.DrawEditingLine(newLine.Length, newLine);
    }
}
