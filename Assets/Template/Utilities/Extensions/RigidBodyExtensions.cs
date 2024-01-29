using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public static class RigidBodyExtensions
{
    public static async UniTask ControlledPush(this Rigidbody rigidBody, Vector3 pushDirection, float pushDistance, float pushSpeed,
        AnimationCurve pushCurve, Action<float> onPushProgress = null, bool evaluatePushProgressByCurve = true,
        CancellationToken cancellationToken = default)
    {
        Vector3 startingPosition = rigidBody.position;

        while (Vector3.Distance(startingPosition, rigidBody.position) < pushDistance)
        {
            float t = Vector3.Distance(startingPosition, rigidBody.position) / pushDistance;
            float currentDashSpeed = Mathf.Lerp(0, pushSpeed, pushCurve.Evaluate(t));

            onPushProgress?.Invoke(evaluatePushProgressByCurve ? pushCurve.Evaluate(t) : t);

            rigidBody.velocity = pushDirection.normalized * currentDashSpeed;
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate, cancellationToken);
        }

        onPushProgress?.Invoke(1);
    }
}