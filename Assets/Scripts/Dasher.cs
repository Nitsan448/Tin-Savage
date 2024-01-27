using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using CharacterController = Platformer3D.CharacterController;

public class Dasher : MonoBehaviour
{
    [HideInInspector] public bool Dashing;
    private CharacterController _controller;
    private LookAtMouse _lookAtMouse;
    private KeyManager _keyManager;
    private Knocker _knocker;
    private BoxCollider _dashCollider;
    [SerializeField] private float _dashDistance;
    [SerializeField] private float _maxDashSpeed;
    [SerializeField] private float _dashChargeTime;
    [SerializeField] private AnimationCurve _dashCurve;
    [SerializeField] private Transform _rig;
    [SerializeField] private float _chargedDashYPosition;
    [SerializeField] private float _chargedDashXRotation;


    private void Awake()
    {
        _keyManager = GetComponent<KeyManager>();
        _lookAtMouse = GetComponent<LookAtMouse>();
        _controller = GetComponent<CharacterController>();
        _knocker = GetComponent<Knocker>();
        _dashCollider = GetComponent<BoxCollider>();
    }

    public async UniTask Dash()
    {
        _lookAtMouse.SetEnabledState(false);
        _knocker.BeingKnocked = false;
        Dashing = true;
        _dashCollider.enabled = true;
        _controller.SetVelocity(Vector3.zero);
        await ChargeDash();
        _keyManager.DropKey();
        Vector3 startingPosition = transform.position;
        Vector3 targetDirection = transform.forward;

        while (Vector3.Distance(startingPosition, transform.position) < _dashDistance - 0.2f)
        {
            float t = Vector3.Distance(startingPosition, transform.position) / _dashDistance;
            float currentDashSpeed = Mathf.Lerp(0, _maxDashSpeed, _dashCurve.Evaluate(t));

            SetRigTransform(1 - t);

            _controller.SetVelocity(targetDirection * currentDashSpeed);
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
        }

        SetRigTransform(0);
        Dashing = false;
        _dashCollider.enabled = false;
        _lookAtMouse.SetEnabledState(true);
    }

    private async UniTask ChargeDash()
    {
        float passedTime = 0;

        while (passedTime < _dashChargeTime)
        {
            float t = passedTime / _dashChargeTime;
            SetRigTransform(t);
            passedTime += Time.deltaTime;
            await UniTask.Yield();
        }

        SetRigTransform(1);
    }

    private void SetRigTransform(float t)
    {
        _rig.localPosition = new Vector3(0, Mathf.Lerp(0, _chargedDashYPosition, t), 0);
        _rig.localEulerAngles = new Vector3(Mathf.Lerp(0, _chargedDashXRotation, t), 0, 0);
    }
}