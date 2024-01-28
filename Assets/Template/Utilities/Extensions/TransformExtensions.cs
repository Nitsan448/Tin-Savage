using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    public static void RotateTowardsOnYAxis(this Transform transformToRotate, Vector3 positionToRotateTowards, float rotationSpeed)
    {
        Quaternion lookRotation = transformToRotate.GetRotationTowardsOnYAxis(positionToRotateTowards);
        transformToRotate.rotation = Quaternion.Lerp(transformToRotate.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    public static Quaternion GetRotationTowardsOnYAxis(this Transform transformToRotate, Vector3 positionToRotateTowards)
    {
        positionToRotateTowards = positionToRotateTowards.GetWithNewYValue(transformToRotate.position.y);
        Vector3 direction = (positionToRotateTowards - transformToRotate.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        return lookRotation;
    }
}