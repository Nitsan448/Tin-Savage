using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyKnocker : MonoBehaviour
{
    [SerializeField] private float _maxKnockBackSpeed = 20;
    [SerializeField] private float _knockBackDistance = 20;
    [SerializeField] private AnimationCurve _knockBackCurve;
    [HideInInspector] public bool BeingKnocked = false;

    private Rigidbody _rigidbody;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public async UniTask Knock(Vector3 knockingObjectPosition)
    {
        if (BeingKnocked)
        {
            return;
        }

        BeingKnocked = true;
        Vector3 startingPosition = transform.position;
        Vector3 startingRotation = transform.eulerAngles;
        knockingObjectPosition = new Vector3(knockingObjectPosition.x, transform.position.y, knockingObjectPosition.z);
        Vector3 targetDirection =
            new Vector3((transform.position - knockingObjectPosition).x, 0, (transform.position - knockingObjectPosition).z).normalized;
        targetDirection = -transform.forward;
        targetDirection.Normalize();
        Debug.Log(targetDirection);
        while (Vector3.Distance(startingPosition, transform.position) < _knockBackDistance - 0.2f)
        {
            float t = Vector3.Distance(startingPosition, transform.position) / _knockBackDistance;
            transform.eulerAngles =
                Vector3.Lerp(startingRotation, new Vector3(startingRotation.x, startingRotation.y + 360, startingRotation.z),
                    _knockBackCurve.Evaluate(t));
            float currentDashSpeed = Mathf.Lerp(0, _maxKnockBackSpeed, _knockBackCurve.Evaluate(t));

            // _rigidbody.AddForce(targetDirection * currentDashSpeed);
            _rigidbody.velocity = targetDirection * currentDashSpeed;
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
        }

        BeingKnocked = false;
    }
}