using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ShootObject : MonoBehaviour
{
    private LookAtMouse _lookAtMouse;
    private bool _aim = false;
    private float _currentRotationVelocity;

    private void Awake()
    {
        _lookAtMouse = GetComponent<LookAtMouse>();
    }

    // Update is called once per frame
    void Update()
    {
        _lookAtMouse.GetCurrentRotationVelocity();
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
        _lookAtMouse.enabled = false;
        float currentRotationVelocity = _lookAtMouse.GetCurrentRotationVelocity();
        while (_aim)
        {
            transform.Rotate(Vector3.up, currentRotationVelocity);
            currentRotationVelocity += Time.deltaTime;
            await UniTask.Yield();
        }
    }

    private void Shoot()
    {
        _aim = false;
        _lookAtMouse.enabled = true;
    }
}