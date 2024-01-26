using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;
    private Vector3 _startingDistanceFromTarget;

    private void Start()
    {
        _startingDistanceFromTarget = transform.position - _target.position;
        Debug.Log(_startingDistanceFromTarget);
    }

    private void LateUpdate()
    {
        transform.position = _startingDistanceFromTarget + _target.position;
    }
}