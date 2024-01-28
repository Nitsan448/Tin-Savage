using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMouse : MonoBehaviour
{
    [SerializeField, Range(0, 30)] private float _rotationSpeed;

    public bool _enabled = true;

    private void FixedUpdate()
    {
        if (!_enabled) return;

        transform.RotateTowards(SceneReferencer.Instance.Player.GetMousePosition(), _rotationSpeed);
    }


    public void SetEnabledState(bool enabled)
    {
        _enabled = enabled;
    }
}