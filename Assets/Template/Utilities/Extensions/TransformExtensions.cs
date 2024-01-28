using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    public static void RotateTowards(this Transform transformToRotate, Vector3 positionToRotateTowards, float rotationSpeed)
    {
        Vector3 direction = positionToRotateTowards - transformToRotate.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        lookRotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
        transformToRotate.rotation = Quaternion.Lerp(transformToRotate.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }
}