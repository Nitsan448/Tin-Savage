using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMouse : MonoBehaviour
{
    [SerializeField, Range(0, 30)] private float _rotationSpeed;

    private Vector3 _worldPosition;
    Plane _plane = new(Vector3.up, 0);

    private Vector3 _previousEulerAngles;
    public bool _enabled = true;
    private Vector3 _worldPositionOnDisable;

    private void FixedUpdate()
    {
        if (_enabled)
        {
            _worldPosition = Vector3.Lerp(_worldPosition, GetMousePosition(), _rotationSpeed * Time.deltaTime);

            Vector3 direction = _worldPosition - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            lookRotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);

            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, _rotationSpeed * Time.deltaTime);
        }
    }


    private Vector3 GetMousePosition()
    {
        _plane.distance = -transform.position.y;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (_plane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }

        return Vector3.zero;
    }


    public void SetEnabledState(bool enabled)
    {
        _enabled = enabled;
    }
}