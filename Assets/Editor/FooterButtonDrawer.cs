using UnityEngine;
using System;

public class FooterButtonDrawer
{
    public void Draw(string buttonText, Action action)
    {
        if (GUILayout.Button(buttonText))
        {
            action.Invoke();
        }
    }
}
