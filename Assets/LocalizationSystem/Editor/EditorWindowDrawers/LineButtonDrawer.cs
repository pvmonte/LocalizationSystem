using System;
using UnityEngine;

public class LineButtonDrawer
{
    public void Draw(string buttonText, Action action)
    {
        if (GUILayout.Button(buttonText))
        {
            action.Invoke();
        }
    }
}
