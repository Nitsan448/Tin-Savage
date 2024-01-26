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
    [SerializeField] private float _maxDashSpeed;
    [SerializeField] private float _dashChargeTime;
    [SerializeField] private AnimationCurve _dashCurve;
    [SerializeField] private Transform _rig;
    [SerializeField] private float _chargedDashYPosition;
    [SerializeField] private float _chargedDashXRotation;

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
        await ChargeDash();
        Vector3 startingPosition = transform.position;
        Vector3 targetDirection = transform.forward;
        float passedTime = 0;

        while (Vector3.Distance(startingPosition, transform.position) < _dashDistance - 0.2f)
        {
            float t = Vector3.Distance(startingPosition, transform.position) / _dashDistance;
            float currentDashSpeed = Mathf.Lerp(0, _maxDashSpeed, _dashCurve.Evaluate(t));
            _rig.localPosition = new Vector3(0, Mathf.Lerp(_chargedDashYPosition, 0, t), 0);
            _rig.localEulerAngles = new Vector3(Mathf.Lerp(_chargedDashXRotation, 0, t), 0, 0);
            _controller.SetVelocity(targetDirection * currentDashSpeed);
            passedTime += Time.fixedDeltaTime;
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
        }

        _rig.localPosition = Vector3.zero;
        _rig.localEulerAngles = Vector3.zero;
        _dashing = false;
        _lookAtMouse.Enabled = true;
    }

    private async UniTask ChargeDash()
    {
        float passedTime = 0;

        while (passedTime < _dashChargeTime)
        {
            float t = passedTime / _dashChargeTime;
            _rig.localPosition = new Vector3(0, Mathf.Lerp(0, _chargedDashYPosition, t), 0);
            _rig.localEulerAngles = new Vector3(Mathf.Lerp(0, _chargedDashXRotation, t), 0, 0);
            passedTime += Time.deltaTime;
            await UniTask.Yield();
        }

        _rig.localPosition = new Vector3(0, _chargedDashYPosition, 0);
        _rig.localEulerAngles = new Vector3(_chargedDashXRotation, 0, 0);
    }
}