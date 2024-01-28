using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerKnocker : MonoBehaviour
{
    [SerializeField] private float _maxKnockBackSpeed;
    [SerializeField] private AnimationCurve _knockBackCurve;

    [SerializeField] private Transform _rig;

    [HideInInspector] public bool BeingKnocked = false;
    private CharacterController _controller;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    public async UniTask Knock(Vector3 knockingObjectPosition, float knockBackDistance)
    {
        BeingKnocked = true;
        Vector3 startingPosition = transform.position;
        Vector3 startingRotation = transform.eulerAngles;
        Vector3 targetDirection = (transform.position - knockingObjectPosition).normalized;

        while (Vector3.Distance(startingPosition, transform.position) < knockBackDistance - 0.2f)
        {
            float t = Vector3.Distance(startingPosition, transform.position) / knockBackDistance;
            transform.eulerAngles =
                Vector3.Lerp(startingRotation, new Vector3(startingRotation.x, startingRotation.y + 360, startingRotation.z),
                    _knockBackCurve.Evaluate(t));
            float currentDashSpeed = Mathf.Lerp(0, _maxKnockBackSpeed, _knockBackCurve.Evaluate(t));

            _controller.SetVelocity(targetDirection * currentDashSpeed);
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
        }

        BeingKnocked = false;
    }
}