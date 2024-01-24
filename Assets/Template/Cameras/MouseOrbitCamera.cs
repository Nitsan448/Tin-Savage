using UnityEngine;
using System;
using UnityEngine.Serialization;

public class MouseCameraOrbit : MonoBehaviour
{
    [SerializeField] private float _cameraDistance = 10f;
    [SerializeField] private float _mouseSensitivity = 4f;

    [SerializeField] private float _scrollSensitivity = 2f;

    [SerializeField] private float _orbitDampening = 10f;
    [SerializeField] private float _scrollDampening = 6f;
    [SerializeField] private Vector2 _distanceRange = new(1.5f, 10f);

    private Quaternion _quaternion;
    private Vector3 _localRotation;
    private float _startingCameraDistance;
    private bool _allowRotation;


    private void Start()
    {
        _localRotation = new Vector3(0, 20, 0);
        _startingCameraDistance = _cameraDistance;
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            UpdateRotation();
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            UpdateZoom();
        }

        if (Input.GetMouseButton(2))
        {
            //Allow moving around
        }

        UpdateCameraTransform();
    }

    private void UpdateRotation()
    {
        if (Input.GetAxis("Mouse X") == 0 && Input.GetAxis("Mouse Y") == 0) return;

        _localRotation.x += Input.GetAxis("Mouse X") * _mouseSensitivity;
        _localRotation.y -= Input.GetAxis("Mouse Y") * _mouseSensitivity;
    }

    private void UpdateZoom()
    {
        float scrollAmount = Input.GetAxis("Mouse ScrollWheel") * _scrollSensitivity;

        //Scroll faster the further we are from the object
        scrollAmount *= (_cameraDistance * 0.3f);
        _cameraDistance += scrollAmount * -1f;
        _cameraDistance = Mathf.Clamp(_cameraDistance, _distanceRange.x, _distanceRange.y);
    }

    private void UpdateCameraTransform()
    {
        _quaternion = Quaternion.Euler(_localRotation.y, _localRotation.x, 0);
        transform.parent.localRotation =
            Quaternion.Lerp(transform.parent.localRotation, _quaternion, Time.deltaTime * _orbitDampening);

        //Only update if camera changed
        if (transform.localPosition.z != _cameraDistance * -1f)
        {
            transform.localPosition = new Vector3(0f, 0f,
                Mathf.Lerp(transform.localPosition.z, _cameraDistance * -1f, Time.deltaTime * _scrollDampening));
        }
    }

    public void ResetCamera()
    {
        //Called from reset camera button.
        transform.parent.localRotation = Quaternion.Euler(0, 0, 0);
        _localRotation = Vector3.zero;
        _cameraDistance = _startingCameraDistance;
        transform.localPosition = new Vector3(0f, 0f,
            Mathf.Lerp(transform.localPosition.z, _cameraDistance * -1f, Time.deltaTime * _scrollDampening));
    }
}