using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : ASingleton<InputManager>
{
    [HideInInspector] public Vector2 DirectionalInputRaw;
    [HideInInspector] public Vector2 DirectionalInput;
    [HideInInspector] public bool JumpInputDown;
    [HideInInspector] public bool JumpInputUp;

    [SerializeField] private KeyCode[] _jumpInputKeyCodes;

    private void Update()
    {
        if (GameManager.Instance.State != EGameState.Running) return;

        DirectionalInputRaw = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        DirectionalInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        GetJumpInput();
    }

    private void GetJumpInput()
    {
        foreach (KeyCode keyCode in _jumpInputKeyCodes)
        {
            JumpInputDown = Input.GetKeyDown(keyCode);
            JumpInputUp = Input.GetKeyUp(keyCode);
            if (JumpInputDown || JumpInputUp)
            {
                break;
            }
        }
    }
}