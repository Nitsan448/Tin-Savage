using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollowPlayer : MonoBehaviour
{
    private const float ForcePower = 10;
    [SerializeField, Range(0, 50)] private float _movementSpeed = 6f;
    [SerializeField, Range(0, 50)] private float _seperateSpeed = 6f;
    [SerializeField, Range(0, 50)] private float _movementAccuracy = 6f;


    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 direction = (SceneReferencer.Instance.Player.transform.position - transform.position).normalized;
        direction += (SeperateFromOtherEnemies() * _seperateSpeed).normalized;
        Vector3 desiredVelocity = direction * _movementSpeed;
        Vector3 deltaVelocity = desiredVelocity - _rigidbody.velocity;
        Vector3 moveForce = deltaVelocity * (_movementAccuracy * ForcePower * Time.fixedDeltaTime);
        // transform.LookAt(direction + transform.position);
        // transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        _rigidbody.AddForce(moveForce);
    }

    private Vector3 SeperateFromOtherEnemies()
    {
        float separateRadius = 10;

        Vector2 finalDirection = Vector2.zero;
        int count = 0;

        var hits = Physics.OverlapSphere(transform.position, separateRadius);
        foreach (var hit in hits)
        {
            if (hit.GetComponent<Enemy>() != null && hit.transform != transform)
            {
                Vector2 direction = transform.position - hit.transform.position;
                direction = direction.normalized / Mathf.Abs(direction.magnitude);
                finalDirection += direction;
                count++;
            }
        }

        if (count == 0) return Vector3.zero;

        finalDirection.Normalize();

        return finalDirection;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, 10);
    }
}