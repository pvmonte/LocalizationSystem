public class AddingState : EditingState, IState
{
    public AddingState(LocalizationWindow window) : base(window)
    {
    }

    public void DrawEditingColumnButtons(string[] lines)
    {
        //DO Nothing
    }

    public void DrawFooter()
    {
        var footerButton = new FooterButtonDrawer();

        footerButton.Draw("Save", () =>
        {
            string addingLine = string.Join(",", newLine);
            window.csvLoader.AddLine(addingLine);
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
            var lineDrawer = new LineDrawer(new CellDrawer());
            lineDrawer.Draw(lines[i]);
        }

        LineEditing(lines[0]);
    }
}