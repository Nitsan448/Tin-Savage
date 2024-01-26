using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollowPlayer : MonoBehaviour
{
    private const float ForcePower = 10;
    [SerializeField, Range(0, 50)] private float _movementSpeed = 6f;
    [SerializeField, Range(0, 50)] private float _movementAccuracy = 6f;


    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 direction = (SceneReferencer.Instance.Player.transform.position - transform.position).normalized;
        Vector3 desiredVelocity = direction * _movementSpeed;
        Vector3 deltaVelocity = desiredVelocity - _rigidbody.velocity;
        Vector3 moveForce = deltaVelocity * (_movementAccuracy * ForcePower * Time.fixedDeltaTime);
        transform.LookAt(direction);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        _rigidbody.AddForce(moveForce);
    }
}