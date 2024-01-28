using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRigController : MonoBehaviour
{
    [SerializeField] private float _chargedDashYPosition;
    [SerializeField] private float _chargedDashXRotation;
    [SerializeField] private Transform _rig;

    public void SetRigTransformDuringDash(float t)
    {
        SetRigTransformDuringDashCharge(1 - t);
    }

    public void SetRigTransformDuringDashCharge(float t)
    {
        _rig.localPosition = new Vector3(0, Mathf.Lerp(0, _chargedDashYPosition, t), 0);
        _rig.localEulerAngles = new Vector3(Mathf.Lerp(0, _chargedDashXRotation, t), 0, 0);
    }
}