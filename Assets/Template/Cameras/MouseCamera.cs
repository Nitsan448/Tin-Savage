using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Camera))]
public class MouseCamera : MonoBehaviour
{
    //TODO: Add support for vertical movement

    [SerializeField] private float _xSensitivity = 0.02f;

    [SerializeField] private float _minXDistanceToStartMoving = 5;

    private Camera _camera;

    private Vector2 _targetPosition = Vector2.zero;
    private Rect _screenRect;
    [SerializeField] private Bounds _bounds;

    public CancellationTokenSource LerpCancellationTokenSource;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void OnEnable()
    {
        transform.position = new Vector3(transform.position.x, 0, -10);
    }


    private void LateUpdate()
    {
        _screenRect = new Rect(0f, 0f, Screen.width, Screen.height);

        if (_screenRect.Contains(Input.mousePosition))
            _targetPosition = _camera.ScreenToWorldPoint(Input.mousePosition);

        float xPosition = transform.position.x;

        if (Mathf.Abs(transform.position.x - _targetPosition.x) > _minXDistanceToStartMoving)
        {
            xPosition = Mathf.Lerp(xPosition, _targetPosition.x, _xSensitivity);
        }

        Vector2 cameraExtents = _camera.OrthographicBounds().extents;
        float maxTargetPosition = xPosition + cameraExtents.x;
        float minTargetPosition = xPosition - cameraExtents.x;

        if (_bounds.min.x > minTargetPosition || _bounds.max.x < maxTargetPosition)
        {
            xPosition = transform.position.x;
        }

        transform.position = new Vector3(xPosition, transform.position.y, -10);
    }

    //TODO: understand the code and refactor it
    public async UniTask LerpBoundsAndSize(Bounds bounds, int cameraSize, float lerpDuration,
        bool lerpToMinXPosition = true)
    {
        LerpCancellationTokenSource?.Cancel();
        LerpCancellationTokenSource = new CancellationTokenSource();
        _bounds = bounds;
        float currentTime = 0;

        Vector3 startingPosition = transform.position;
        float startingSize = _camera.orthographicSize;
        Vector3 targetPosition;

        if (lerpToMinXPosition)
        {
            while (currentTime < lerpDuration)
            {
                targetPosition = new Vector3(_bounds.max.x - Camera.main.OrthographicBounds().extents.x, 0, -10);
                transform.position = Vector3.Lerp(startingPosition, targetPosition, currentTime / lerpDuration);
                Camera.main.orthographicSize = Mathf.Lerp(startingSize, cameraSize, currentTime / lerpDuration);
                currentTime += Time.deltaTime / 2;
                await UniTask.Yield();
            }

            Camera.main.orthographicSize = cameraSize;
            targetPosition = new Vector3(_bounds.max.x - Camera.main.OrthographicBounds().extents.x, 0, -10);
            transform.position = targetPosition;
        }

        currentTime = 0;
        startingPosition = transform.position;
        while (currentTime < lerpDuration)
        {
            targetPosition = new Vector3(_bounds.min.x + Camera.main.OrthographicBounds().extents.x, 0, -10);
            transform.position = Vector3.Lerp(startingPosition, targetPosition, currentTime / lerpDuration);
            if (!lerpToMinXPosition)
            {
                Camera.main.orthographicSize = Mathf.Lerp(startingSize, cameraSize, currentTime / lerpDuration);
            }

            currentTime += Time.deltaTime;
            await UniTask.Yield();
        }

        if (!lerpToMinXPosition)
        {
            Camera.main.orthographicSize = cameraSize;
        }

        targetPosition = new Vector3(_bounds.min.x + Camera.main.OrthographicBounds().extents.x, 0, -10);
        transform.position = targetPosition;
    }
}