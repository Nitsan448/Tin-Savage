using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

//Can be added to template 2d platformer part, along with character controller
public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public Vector2 FocusAreaSize;
    public float VerticalOffset;
    public float LookAheadDistanceX;
    public float LookSmoothTimeX;
    public float VerticalSmoothTime;

    private FocusArea _focusArea;
    private float _currentLookAheadX;
    private float _targetLookAheadX;
    private float _lookAheadDirectionX;
    private float _smoothLookVelocityX;
    private float _smoothVelocityY;
    private bool _lookAheadStopped;

    private void Start()
    {
        _focusArea = new FocusArea(Target, FocusAreaSize);
    }

    private void LateUpdate()
    {
        _focusArea.Update(Target);

        Vector2 focusPosition = _focusArea.Center + Vector2.up * VerticalOffset;

        if (_focusArea.Velocity.x != 0)
        {
            CalculateTargetLookAheadX();
        }

        _currentLookAheadX = Mathf.SmoothDamp(_currentLookAheadX, _targetLookAheadX, ref _smoothLookVelocityX,
            LookSmoothTimeX);

        focusPosition.y =
            Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref _smoothVelocityY, VerticalSmoothTime);
        focusPosition += Vector2.right * _currentLookAheadX;

        transform.position = (Vector3)focusPosition + Vector3.forward * -10;
    }

    private void CalculateTargetLookAheadX()
    {
        //If the player stops moving in the direction the camera is moving, we lower the camera's movement
        _lookAheadDirectionX = Mathf.Sign(_focusArea.Velocity.x);
        float horizontalInput = InputManager.Instance.DirectionalInputRaw.x;
        if (Mathf.Sign(horizontalInput) == Mathf.Sign(_focusArea.Velocity.x) && horizontalInput != 0)
        {
            _lookAheadStopped = false;
            _targetLookAheadX = _lookAheadDirectionX * LookAheadDistanceX;
        }
        else
        {
            if (!_lookAheadStopped)
            {
                _lookAheadStopped = true;
                _targetLookAheadX = _currentLookAheadX +
                                    (_lookAheadDirectionX * LookAheadDistanceX - _currentLookAheadX) / 4f;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(_focusArea.Center, FocusAreaSize);
    }
}