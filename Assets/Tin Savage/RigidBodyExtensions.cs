using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public static class RigidBodyExtensions
{
    public static async UniTask ControlledPush(this Rigidbody rigidBody, Vector3 pushDirection, float pushDistance, float pushSpeed,
        AnimationCurve pushCurve, Action<float> onPushProgress = null, CancellationTokenSource cts = null)
    {
        Vector3 startingPosition = rigidBody.position;
        //push direction = transform.forward

        while (Vector3.Distance(startingPosition, rigidBody.position) < pushDistance)
        {
            if (cts != null && cts.IsCancellationRequested)
            {
                onPushProgress?.Invoke(1);
                return;
            }

            float t = Vector3.Distance(startingPosition, rigidBody.position) / pushDistance;
            float currentDashSpeed = Mathf.Lerp(0, pushSpeed, pushCurve.Evaluate(t));

            onPushProgress?.Invoke(t);
            rigidBody.velocity = pushDirection * currentDashSpeed;
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
        }

        onPushProgress?.Invoke(1);
    }
}