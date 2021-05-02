public class EditingLineState : EditingState, IState
{

    int editingLineIndex = -1;

    public EditingLineState(LocalizationWindow window, int editingLineIndex) : base(window)
    {
        this.editingLineIndex = editingLineIndex;
    }

    public void DrawEditingColumnButtons(string[] lines)
    {
        //do Nothing
    }

    public void DrawFooter()
    {
        var footerButton = new FooterButtonDrawer();

        footerButton.Draw("Edit", () =>
        {
            string editingLine = string.Join(",", newLine);
            window.csvLoader.
            Edit(editingLineIndex, editingLine);
            window.ChangeToState(new StandardState(window));
            window.Refresh();
        });

        footerButton.Draw("Cancel", () =>
        {
            window.ChangeToState(new StandardState(window));
        });
    }

    public void DrawTable(string[] lines)
    {
        for (int i = 1; i < lines.Length; i++)
        {
            if (editingLineIndex == i)
            {
                LineEditing(lines[editingLineIndex]);
            }
            else
            {
                var lineDrawer = new LineDrawer(new CellDrawer());
                lineDrawer.Draw(lines[i]);
            }
        }
    }
}
