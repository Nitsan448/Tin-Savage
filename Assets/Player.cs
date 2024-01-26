using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using CharacterController = Platformer3D.CharacterController;

public class Player : MonoBehaviour
{
    private CharacterController _controller;
    private LookAtMouse _lookAtMouse;
    private Dasher _dasher;

    private void Awake()
    {
        _dasher = GetComponent<Dasher>();
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (_dasher.Dashing) return;
        _controller.UpdateInput();
        if (Input.GetMouseButtonDown(0) && GameManager.Instance.State == EGameState.Running)
        {
            _dasher.Dash().Forget();
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.State == EGameState.Running && !_dasher.Dashing)
        {
            _controller.CalculateVelocity();
        }
    }
}