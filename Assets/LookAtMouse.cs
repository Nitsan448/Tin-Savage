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

    void Update()
    {
        _worldPosition = Vector3.Lerp(_worldPosition, GetMousePosition(), _rotationSpeed * Time.deltaTime);
        transform.LookAt(_worldPosition);
    }

    private Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (_plane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }

        return Vector3.zero;
    }
}