using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class CursorSetter : MonoBehaviour
{
    private static Texture2D _standardCursor;
    private static Texture2D _inputFieldCursor;
    private static Texture2D _buttonCursor;
    private Selectable[] _selectablesInChildren;

    private void Start()
    {
        _standardCursor = Resources.Load<Texture2D>("Cursors/DefaultCursor");
        // _standardCursor = null;
        _inputFieldCursor = Resources.Load<Texture2D>("Cursors/InputFieldCursor");
        _buttonCursor = Resources.Load<Texture2D>("Cursors/ButtonCursor");
        SetCursorToStandard();
        AddCursorOnHoverScriptToChildren();
    }

    private void AddCursorOnHoverScriptToChildren()
    {
        _selectablesInChildren = GetComponentsInChildren<Selectable>(true);

        foreach (Selectable selectalbe in _selectablesInChildren)
        {
            AddSetCursorOnHoverToGameObject(selectalbe.gameObject);
        }
    }

    private void AddSetCursorOnHoverToGameObject(GameObject selectableGameObject)
    {
        if (selectableGameObject.GetComponent<SetCursorOnHover>() == null)
        {
            selectableGameObject.AddComponent<SetCursorOnHover>();
        }
    }

    public static void SetCursorToStandard()
    {
        Cursor.SetCursor(_standardCursor, new Vector2(128, 0), cursorMode: CursorMode.Auto);
    }

    public static void SetCursorToInputField()
    {
        Cursor.SetCursor(
            _inputFieldCursor,
            new Vector2(_inputFieldCursor.width / 2, _inputFieldCursor.height / 2),
            cursorMode: CursorMode.Auto
        );
    }

    public static void SetCursorToButton()
    {
        Cursor.SetCursor(_buttonCursor, new Vector2(6, 0), cursorMode: CursorMode.Auto);
    }
}