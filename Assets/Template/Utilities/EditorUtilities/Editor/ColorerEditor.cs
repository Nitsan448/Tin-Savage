using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Colorer))]
public class ColorerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Colorer colorer = target as Colorer;

        if (colorer.ColorPallete == null)
        {
            Debug.Log("Loading main color pallete");
            colorer.ColorPallete =
                AssetDatabase.LoadAssetAtPath<ColorPallete>(
                    "Assets/Utilities/EditorUtilities/Colors/MainColorPallete.asset");
            if (colorer.ColorPallete == null)
            {
                Debug.Log("Color Pallete not found, check the path");
                return;
            }
        }

        GUI.color = colorer.SpecificColor;
        if (GUILayout.Button("Color"))
        {
            colorer.Color(colorer.SpecificColor);
        }

        GUILayout.Space(5);
        GUI.color = Color.white;
        GUILayout.Label("Previous Color");
        GUI.color = colorer.PreviousColor;
        if (GUILayout.Button("Color"))
        {
            colorer.Color(colorer.PreviousColor);
        }

        GUILayout.Space(5);
        GUI.color = Color.white;
        GUILayout.Label("Color Pallete");
        foreach (Color color in colorer.ColorPallete.Colors)
        {
            GUI.color = color;
            if (GUILayout.Button("Color"))
            {
                colorer.Color(color);
            }
        }
    }
}