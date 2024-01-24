using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ChildrenRenamer))]
public class ChildrenRenamerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Rename top level children"))
        {
            (target as ChildrenRenamer).RenameTopLevelChildren();
        }
    }
}