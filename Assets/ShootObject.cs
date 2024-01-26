using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public class ShootObject : MonoBehaviour
{
    private LookAtMouse _lookAtMouse;
    private bool _aim = false;
    private float _currentRotationVelocity;
    [SerializeField, Range(0, 2)] private float _rotationAcceleration;
    [SerializeField, Range(0, 3)] private float _rotationSpeed;
    [SerializeField] private Rigidbody _objectToShootRigidbody;
    [SerializeField] private float _shootStrength;

    private void Awake()
    {
        _lookAtMouse = GetComponent<LookAtMouse>();
    }

    // Update is called once per frame
    void Update()
    {
        // _lookAtMouse.GetCurrentRotationVelocity();
        if (Input.GetMouseButtonDown(0))
        {
            Aim().Forget();
        }

        if (Input.GetMouseButtonUp(0))
        {
            Shoot();
        }
    }

    private async UniTask Aim()
    {
        _aim = true;
        _lookAtMouse._enabled = false;
        // _currentRotationVelocity = _lookAtMouse.GetCurrentRotationVelocity() * (1 / _rotationSpeed);
        while (_aim)
        {
            transform.Rotate(Vector3.up, _currentRotationVelocity * _rotationSpeed);
            float velocityChange = Time.deltaTime * _rotationAcceleration;
            _currentRotationVelocity = _currentRotationVelocity > 0
                ? _currentRotationVelocity + velocityChange
                : _currentRotationVelocity - velocityChange;
            await UniTask.Yield();
        }
    }

    private void Shoot()
    {
        _aim = false;
        _lookAtMouse._enabled = true;
        Vector3 force = transform.forward * (_currentRotationVelocity) * _shootStrength;
        if (_currentRotationVelocity < 0)
        {
            force *= -1;
        }

        _objectToShootRigidbody.transform.parent = null;
        _objectToShootRigidbody.isKinematic = false;
        _objectToShootRigidbody.useGravity = true;
        _objectToShootRigidbody.AddForce(force, ForceMode.Impulse);
        // _objectToShootRigidbody.AddTorque(force, ForceMode.Acceleration);
    }
}