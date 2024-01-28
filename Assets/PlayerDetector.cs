using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] private float _distanceToEnterDetectingMode;
    [SerializeField] private float _distanceToExitDetectingMode;

    [SerializeField, Range(0, 50)] private float _movementSpeedMultiplierOnDetection = 2f;
    [SerializeField, Range(0, 50)] private float _movementAccuracyMultiplierOnDetection = 2f;
    [SerializeField, Range(0, 50)] private float _separateSpeedMultiplierOnDetection = 0.5f;

    private ObjectFollowPlayer _objectFollowPlayer;
    private bool _playerDetected;

    public float MovementSpeedMultiplier => _playerDetected ? _movementSpeedMultiplierOnDetection : 1;
    public float MovementAccuracyMultiplier => _playerDetected ? _movementAccuracyMultiplierOnDetection : 1;
    public float SeparateSpeedMultiplierOnDetection => _playerDetected ? _separateSpeedMultiplierOnDetection : 1;

    private void Awake()
    {
        _objectFollowPlayer = GetComponent<ObjectFollowPlayer>();
    }

    void Update()
    {
        if (!_playerDetected && Vector3.Distance(transform.position, SceneReferencer.Instance.Player.transform.position) <
            _distanceToEnterDetectingMode)
        {
            EnterDetectionMode();
        }
        else if (_playerDetected && Vector3.Distance(transform.position, SceneReferencer.Instance.Player.transform.position) <
                 _distanceToExitDetectingMode)
        {
            ExitDetectionMode();
        }
    }

    private void EnterDetectionMode()
    {
        _playerDetected = true;
    }

    private void ExitDetectionMode()
    {
        _playerDetected = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _distanceToEnterDetectingMode);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _distanceToExitDetectingMode);
    }
}