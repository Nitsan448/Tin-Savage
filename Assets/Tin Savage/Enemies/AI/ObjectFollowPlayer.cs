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

    [SerializeField] private float _radiusFromHole = 12;
    [SerializeField, Range(0, 3)] private float _avoidHoleMultiplier;

    private float _separateSpeed;
    private float _separateRadius;
    private float _timeSinceLastSeparateSpeedChanged;
    private float _timeBetweenRandomizations;
    private Rigidbody _rigidbody;
    private float _currentSeparateSpeed;
    private Vector3 _awayFromHoleDirection;

    private bool _moveAwayFromHole;

    private PlayerDetector _playerDetector;
    private Enemy _enemy;


    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _playerDetector = GetComponent<PlayerDetector>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _timeSinceLastSeparateSpeedChanged += Time.deltaTime;
        if (_timeSinceLastSeparateSpeedChanged > _timeBetweenRandomizations)
        {
            _timeBetweenRandomizations = Random.Range(_timeBetweenRandomizationsRange.x, _timeBetweenRandomizationsRange.y);
            _separateSpeed = Random.Range(_seperateSpeedRange.x, _seperateSpeedRange.y) *
                             _playerDetector.SeparateSpeedMultiplierOnDetection;
            _separateRadius = Random.Range(_seperateRadiusRange.x, _seperateRadiusRange.y);
            _timeSinceLastSeparateSpeedChanged = 0;
            _currentSeparateSpeed = _separateSpeed;
        }
    }

    private void FixedUpdate()
    {
        if (_enemy.BeingKnocked)
        {
            return;
        }

        HandleHole();
        Vector3 direction = (SceneReferencer.Instance.Player.transform.position - transform.position).normalized;
        if (_moveAwayFromHole)
        {
            direction = (transform.position - SceneReferencer.Instance.Danger.transform.position).normalized * _avoidHoleMultiplier +
                        direction;
        }
        else
        {
            direction = (direction * (_playerDetector.MovementSpeedMultiplier * _movementSpeed) +
                         SeperateFromOtherEnemies() * _currentSeparateSpeed).normalized;
        }


        Vector3 desiredVelocity = direction.normalized *
                                  (_movementSpeed * _playerDetector.MovementSpeedMultiplier);
        Vector3 deltaVelocity = desiredVelocity - _rigidbody.velocity;
        Vector3 moveForce = deltaVelocity *
                            (_movementAccuracy * _playerDetector.MovementAccuracyMultiplier * ForcePower * Time.fixedDeltaTime);
        moveForce = new Vector3(moveForce.x, 0, moveForce.z);
        _rigidbody.AddForce(moveForce);

        transform.RotateTowardsOnYAxis(SceneReferencer.Instance.Player.transform.position, _rotationSpeed);
    }

    private Vector3 SeperateFromOtherEnemies()
    {
        Vector2 finalDirection = Vector2.zero;
        int count = 0;

        Collider[] enemyHits = Physics.OverlapSphere(transform.position, _separateRadius);
        foreach (var hit in enemyHits)
        {
            if (hit.TryGetComponent(out Enemy enemy) && hit.transform != transform)
            {
                Vector2 direction = transform.position - hit.transform.position;
                direction = direction.normalized / Mathf.Abs(direction.magnitude);
                finalDirection += direction;
                count++;
                _currentSeparateSpeed = _separateSpeed / 2;
            }
        }

        if (count == 0) return Vector3.zero;

        finalDirection.Normalize();

        return finalDirection;
    }

    private void HandleHole()
    {
        if (_moveAwayFromHole && Vector3.Distance(transform.position, SceneReferencer.Instance.Danger.transform.position) >
            _radiusFromHole + 3)
        {
            _moveAwayFromHole = false;
        }

        if (Vector3.Distance(transform.position, SceneReferencer.Instance.Danger.transform.position) < _radiusFromHole)
        {
            _moveAwayFromHole = true;
        }
    }
}