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
    private readonly List<Vector3> _previousRotations = new();
    [HideInInspector] public bool Enabled = true;

    private void FixedUpdate()
    {
        _previousRotations.Add(transform.eulerAngles);
        if (_previousRotations.Count > 5)
        {
            _previousRotations.RemoveAt(0);
        }

        _worldPosition = Vector3.Lerp(_worldPosition, GetMousePosition(), _rotationSpeed * Time.deltaTime);
        if (Enabled)
        {
            transform.LookAt(_worldPosition);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
    }

    private Vector3 GetMousePosition()
    {
        _plane.distance = -transform.position.y;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (_plane.Raycast(ray, out float distance))
        {
            Vector3 mousePosition = ray.GetPoint(distance);
            return ray.GetPoint(distance);
        }

        return Vector3.zero;
    }

    public float GetCurrentRotationVelocity()
    {
        float rotationVelocity = 0;
        for (int index = 1; index < _previousRotations.Count; index++)
        {
            rotationVelocity += _previousRotations[index].y - _previousRotations[index - 1].y;
        }

        rotationVelocity /= _previousRotations.Count;

        return rotationVelocity;
    }
}