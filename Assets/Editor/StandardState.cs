using UnityEngine;
using UnityEditor;

public class StandardState : State, IState
{
    float endLineButtonsSize = 20;

    public StandardState(LocalizationWindow window) : base(window)
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
        EditorGUILayout.BeginVertical(GUILayout.Width(endLineButtonsSize * 2));

        for (int i = 0; i < lines.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();

            var lineButtonDrawer = new LineButtonDrawer();

            if (i == 0)
            {
                lineButtonDrawer.Draw("+", () => window.ChangeToState(new AddingColumnState(window)));
            }
            else
            {                
                lineButtonDrawer.Draw("E", () =>
                {
                    window.ChangeToState(new EditingLineState(window, i));
                });

                string[] keys = new string[lines.Length];
                string key = lines[i].Split(',')[0];

                lineButtonDrawer.Draw("-", () =>
                {
                    window.csvLoader.Remove(key);
                    window.Refresh();
                });
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();
    }

    public void DrawFooter()
    {
        var footerButton = new FooterButtonDrawer();
        footerButton.Draw("+", () => window.ChangeToState(new AddingState(window)));
        footerButton.Draw("Refresh", window.Refresh);
    }
}
