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
    [SerializeField] private float _dashDistance;
    [SerializeField] private float _startingDashSpeed;
    [SerializeField] private float _maxDashSpeed;
    [SerializeField] private float _dashChargeTime;
    [SerializeField] private float _dashVelocityOnEnd;

    private bool _dashing;


    private void Awake()
    {
        _lookAtMouse = GetComponent<LookAtMouse>();
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!_dashing)
        {
            _controller.UpdateInput();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Dash().Forget();
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.State == EGameState.Running && !_dashing)
        {
            _controller.CalculateVelocity();
        }
        else if (GameManager.Instance.State == EGameState.Paused)
        {
            // _controller.RigidBody.velocity = Vector3.zero;
        }
    }

    private async UniTask Dash()
    {
        Debug.Log(_dashing);
        if (_dashing)
        {
            return;
        }

        _lookAtMouse.Enabled = false;
        _dashing = true;
        _controller.SetVelocity(Vector3.zero);
        await UniTask.Delay(TimeSpan.FromSeconds(_dashChargeTime));
        Vector3 startingPosition = transform.position;
        Vector3 targetDirection = transform.forward;
        float passedTime = 0;

        _controller.SetVelocity(targetDirection * _startingDashSpeed);
        while (Vector3.Distance(startingPosition, transform.position) < _dashDistance)
        {
            float t = Vector3.Distance(startingPosition, transform.position) / _dashDistance / 2;
            float currentDashSpeed = Mathf.Lerp(_startingDashSpeed, _maxDashSpeed, t);

            _controller.SetVelocity(targetDirection * Mathf.Lerp(_startingDashSpeed, _maxDashSpeed, t));
            _controller.SetVelocity(targetDirection * _startingDashSpeed);
            passedTime += Time.fixedDeltaTime;
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
        }

        // _controller.SetVelocity(targetDirection * _dashVelocityOnEnd);
        _controller.SetVelocity(Vector3.zero);

        Debug.Log("here");
        _dashing = false;
        _lookAtMouse.Enabled = true;
    }
}