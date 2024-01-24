using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomGUIStyles
{
    public static GUIStyle BoldMultiLineLabel = new GUIStyle()
    {
        wordWrap = true,
        fontStyle = FontStyle.Bold
    };
    public static GUIStyle ErrorLabel = new GUIStyle()
    {
        wordWrap = true,
        normal = new GUIStyleState() { textColor = Color.red }
    };
}
