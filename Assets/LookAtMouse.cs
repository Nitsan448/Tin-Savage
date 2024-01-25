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

    void Update()
    {
        _previousRotations.Add(transform.eulerAngles);
        if (_previousRotations.Count > 10)
        {
            _previousRotations.RemoveAt(0);
        }

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

    //TODO: return to this after figuring out shoot object
    public float GetCurrentRotationVelocity()
    {
        float rotationVelocity = 0;
        for (int index = 1; index < _previousRotations.Count; index++)
        {
            rotationVelocity += _previousRotations[index].y - _previousRotations[index - 1].y;
        }

        rotationVelocity /= _previousRotations.Count;

        Debug.Log(rotationVelocity);
        return rotationVelocity;
    }
}