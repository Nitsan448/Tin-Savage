using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectFollowPlayer : MonoBehaviour
{
    private const float ForcePower = 10;
    [SerializeField, Range(0, 50)] private float _movementSpeed = 6f;
    [SerializeField, Range(0, 50)] private float _movementAccuracy = 6f;
    [SerializeField, Range(0, 50)] private float _rotationSpeed = 6f;

    [Header("Randomization")] [SerializeField]
    private Vector2 _timeBetweenRandomizationsRange;

    [SerializeField] private Vector2 _seperateSpeedRange;
    [SerializeField] private Vector2 _seperateRadiusRange;

    private float _separateSpeed;
    private float _separateRadius;
    private float _timeSinceLastSeparateSpeedChanged;
    private float _timeBetweenRandomizations;
    private Rigidbody _rigidbody;
    private Vector3 nextRotation;
    private float _timeSinceLookAtRotationSaved = 0;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _timeSinceLastSeparateSpeedChanged += Time.deltaTime;
        if (_timeSinceLastSeparateSpeedChanged > _timeBetweenRandomizations)
        {
            _timeBetweenRandomizations = Random.Range(_timeBetweenRandomizationsRange.x, _timeBetweenRandomizationsRange.y);
            _separateSpeed = Random.Range(_seperateSpeedRange.x, _seperateSpeedRange.y);
            _separateRadius = Random.Range(_seperateRadiusRange.x, _seperateRadiusRange.y);
            _timeSinceLastSeparateSpeedChanged = 0;
        }

        _timeSinceLookAtRotationSaved += Time.deltaTime;
        if (_timeSinceLookAtRotationSaved > 0.2f)
        {
            // _timeSinceLookAtRotationSaved = 
        }
    }

    private void FixedUpdate()
    {
        Vector3 direction = (SceneReferencer.Instance.Player.transform.position - transform.position).normalized;
        direction += (SeperateFromOtherEnemies() * _separateSpeed).normalized;
        Vector3 desiredVelocity = direction * _movementSpeed;
        Vector3 deltaVelocity = desiredVelocity - _rigidbody.velocity;
        Vector3 moveForce = deltaVelocity * (_movementAccuracy * ForcePower * Time.fixedDeltaTime);
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        lookRotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, _rotationSpeed * Time.deltaTime);
        // transform.LookAt(direction);
        // transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        _rigidbody.AddForce(moveForce);
    }

    private Vector3 SeperateFromOtherEnemies()
    {
        Vector2 finalDirection = Vector2.zero;
        int count = 0;

        var hits = Physics.OverlapSphere(transform.position, _separateRadius);
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
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _seperateRadiusRange.x);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _seperateRadiusRange.y);
    }
}