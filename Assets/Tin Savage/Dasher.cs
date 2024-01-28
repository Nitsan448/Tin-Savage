using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using CharacterController = Platformer3D.CharacterController;

public class Dasher : MonoBehaviour
{
    private CharacterController _controller;

    // private BoxCollider _dashCollider;
    [SerializeField] private float _dashDistance;
    [SerializeField] private float _maxDashSpeed;
    [SerializeField] private float _dashChargeTime;
    [SerializeField] private AnimationCurve _dashCurve;
    [SerializeField] private Transform _rig;
    [SerializeField] private float _chargedDashYPosition;
    [SerializeField] private float _chargedDashXRotation;

    [SerializeField] private GameObject _dashTrail;
    [HideInInspector] public int DashScore = 0;


    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    public async UniTask Dash()
    {
        Quaternion lookRotation = transform.GetRotationTowardsOnYAxis(SceneReferencer.Instance.Player.GetMousePosition());
        transform.rotation = lookRotation;
        Vector3 startingPosition = transform.position;
        Vector3 targetDirection = transform.forward;

        while (Vector3.Distance(startingPosition, transform.position) < _dashDistance - 0.2f)
        {
            if (!SceneReferencer.Instance.Player.Dashing)
            {
                FinishDash();
                return;
            }

            float t = Vector3.Distance(startingPosition, transform.position) / _dashDistance;
            float currentDashSpeed = Mathf.Lerp(0, _maxDashSpeed, _dashCurve.Evaluate(t));

            SetRigTransform(1 - t);
            SceneReferencer.Instance.Player.Controller.SetVelocity(targetDirection * currentDashSpeed);
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
        }

        FinishDash();
    }

    private void FinishDash()
    {
        SetRigTransform(0);
        DashScore = 0;
    }

    public async UniTask ChargeDash()
    {
        float passedTime = 0;

        AudioManager.Instance.Play("DashCharge");
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