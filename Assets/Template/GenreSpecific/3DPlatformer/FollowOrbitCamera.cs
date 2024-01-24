using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer3D
{
    [RequireComponent(typeof(Camera))]
    public class FollowOrbitCamera : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField, Range(1f, 20f)] private float _distanceFromTarget = 5f;
        [SerializeField, Min(0f)] private float _focusRadius = 1f;
        [SerializeField, Range(0f, 1f)] private float _focusCentering = 0.5f;
        [SerializeField, Range(1, 360)] private float _rotationSpeed = 90;
        [SerializeField, Range(-89f, 89f)] float _minVerticalAngle = -30f, _maxVerticalAngle = 60f;
        [SerializeField, Min(0f)] private float _alignDelay = 5f;
        [SerializeField, Range(0, 90)] private float _alignSmoothRange = 45f;
        [SerializeField] private LayerMask _obstructionMask = -1;

        private Vector3 _focusPoint;
        private Vector3 _previousFocusPoint;
        private Vector2 _orbitAngles = new(45, 0);
        private float _lastManualRotationTime;
        private Camera _regularCamera;

        private void Awake()
        {
            _regularCamera = GetComponent<Camera>();
            _focusPoint = _target.position;
            transform.localRotation = Quaternion.Euler(_orbitAngles);
        }

#if !UNITY_EDITOR
        private void OnEnable()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
#endif

        void LateUpdate()
        {
            UpdateFocusPoint();
            Quaternion lookRotation;
            if (ManualRotation() || AutomaticRotation())
            {
                ConstrainAngles();
                lookRotation = Quaternion.Euler(_orbitAngles);
            }
            else
            {
                lookRotation = transform.localRotation;
            }

            Vector3 lookDirection = lookRotation * Vector3.forward;

            //Moving away from the focus position in the opposite direction that it's looking by configured distance
            Vector3 lookPosition = _focusPoint - lookDirection * _distanceFromTarget;

            Vector3 rectOffset = lookDirection * _regularCamera.nearClipPlane;
            Vector3 rectPosition = lookPosition + rectOffset;
            Vector3 castFrom = _target.position;
            Vector3 castLine = rectPosition - castFrom;
            float castDistance = castLine.magnitude;
            Vector3 castDirection = castLine / castDistance;

            if (Physics.BoxCast(castFrom, CameraHalfExtends, castDirection, out RaycastHit hit, lookRotation,
                    castDistance, _obstructionMask))
            {
                rectPosition = castFrom + castDirection * hit.distance;
                lookPosition = rectPosition - rectOffset;
            }

            transform.SetPositionAndRotation(lookPosition, lookRotation);
        }

        //TODO: understand this method better
        private void UpdateFocusPoint()
        {
            _previousFocusPoint = _focusPoint;
            Vector3 targetPoint = _target.position;
            float distance = Vector3.Distance(_focusPoint, targetPoint);
            float t = 1;
            if (distance > 0.01f && _focusCentering > 0)
            {
                t = Mathf.Pow(1 - _focusCentering, Time.unscaledDeltaTime);
            }

            if (distance > _focusRadius)
            {
                t = Mathf.Min(t, _focusRadius / distance);
            }

            _focusPoint = Vector3.Lerp(targetPoint, _focusPoint, t);
        }

        private bool ManualRotation()
        {
            //TODO: add to input manager
            Vector2 input = new(Input.GetAxis("Vertical Camera"), Input.GetAxis("Horizontal Camera"));
            const float e = 0.001f;
            if (input.x < -e || input.x > e || input.y < -e || input.y > e)
            {
                _orbitAngles += _rotationSpeed * Time.unscaledDeltaTime * input;
                _lastManualRotationTime = Time.unscaledTime;
                return true;
            }

            return false;
        }

        private bool AutomaticRotation()
        {
            if (Time.unscaledTime - _lastManualRotationTime < _alignDelay)
            {
                return false;
            }

            Vector2 movement = new(_focusPoint.x - _previousFocusPoint.x, _focusPoint.z - _previousFocusPoint.z);
            float movementDeltaSquared = movement.sqrMagnitude;
            if (movementDeltaSquared < 0.0001f)
            {
                return false;
            }

            float headingAngle = GetAngle(movement / Mathf.Sqrt(movementDeltaSquared));
            float deltaAbs = Mathf.Abs(Mathf.DeltaAngle(_orbitAngles.y, headingAngle));
            float rotationChange = _rotationSpeed * Mathf.Min(Time.unscaledDeltaTime, movementDeltaSquared);
            if (deltaAbs < _alignSmoothRange)
            {
                rotationChange *= deltaAbs / _alignSmoothRange;
            }
            else if (180f - deltaAbs < _alignSmoothRange)
            {
                rotationChange *= (180f - deltaAbs) / _alignSmoothRange;
            }

            _orbitAngles.y = Mathf.MoveTowardsAngle(_orbitAngles.y, headingAngle, rotationChange);
            return true;
        }

        private static float GetAngle(Vector2 direction)
        {
            //TODO: understand this better
            float angle = Mathf.Acos(direction.y) * Mathf.Rad2Deg;
            return direction.x < 0 ? 360f - angle : angle;
        }

        private void ConstrainAngles()
        {
            _orbitAngles.x =
                Mathf.Clamp(_orbitAngles.x, _minVerticalAngle, _maxVerticalAngle);

            if (_orbitAngles.y < 0f)
            {
                _orbitAngles.y += 360f;
            }
            else if (_orbitAngles.y >= 360f)
            {
                _orbitAngles.y -= 360f;
            }
        }

        Vector3 CameraHalfExtends
        {
            get
            {
                Vector3 halfExtends;
                halfExtends.y = _regularCamera.nearClipPlane * Mathf.Tan(0.5f * Mathf.Deg2Rad * _regularCamera.fieldOfView);
                halfExtends.x = halfExtends.y * _regularCamera.aspect;
                halfExtends.z = 0f;
                return halfExtends;
            }
        }


        private void OnValidate()
        {
            if (_maxVerticalAngle < _minVerticalAngle)
            {
                _maxVerticalAngle = _minVerticalAngle;
            }
        }
    }
}