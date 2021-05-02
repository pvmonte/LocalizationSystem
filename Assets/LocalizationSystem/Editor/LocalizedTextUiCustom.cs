using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LocalizedUIText))]
public class LocalizedTextUiCustom : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var uiText = (LocalizedUIText)target;
        var keys = LocalizationWindow.thisWindow.csvLoader.keys;

        
        uiText.keyIndex = EditorGUILayout.Popup("Key", uiText.keyIndex, keys);
        uiText.key = keys[uiText.keyIndex];
    }
}
