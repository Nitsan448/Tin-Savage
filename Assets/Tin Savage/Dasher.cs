using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using CharacterController = Platformer3D.CharacterController;

public class Dasher : MonoBehaviour
{
    [HideInInspector] public bool Dashing;
    private CharacterController _controller;
    private LookAtMouse _lookAtMouse;
    private KeyManager _keyManager;

    private PlayerKnocker _playerKnocker;

    // private BoxCollider _dashCollider;
    [SerializeField] private float _dashDistance;
    [SerializeField] private float _maxDashSpeed;
    [SerializeField] private float _dashChargeTime;
    [SerializeField] private AnimationCurve _dashCurve;
    [SerializeField] private Transform _rig;
    [SerializeField] private float _chargedDashYPosition;
    [SerializeField] private float _chargedDashXRotation;

    [SerializeField] private GameObject _dashTrail;
    [HideInInspector] public int DashScore = 0;


    private void Awake()
    {
        _keyManager = GetComponent<KeyManager>();
        _lookAtMouse = GetComponent<LookAtMouse>();
        _controller = GetComponent<CharacterController>();
        _playerKnocker = GetComponent<PlayerKnocker>();
    }

    public async UniTask Dash()
    {
        DashScore = 0;
        _lookAtMouse.SetEnabledState(false);
        _playerKnocker.BeingKnocked = false;
        Dashing = true;
        _controller.RigidBody.isKinematic = true;
        _dashTrail.SetActive(true);
        _keyManager.KeyAnimator.SetTrigger("Charge");
        _controller.SetVelocity(Vector3.zero);

        Vector3 direction = GetMousePosition() - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        lookRotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
        transform.rotation = lookRotation;

        await ChargeDash();
        Debug.Log(transform.eulerAngles);
        _controller.RigidBody.isKinematic = false;
        AudioManager.Instance.Play("Dash");
        _keyManager.DropKey();
        transform.rotation = lookRotation;
        Vector3 startingPosition = transform.position;
        Vector3 targetDirection = transform.forward;

        while (Vector3.Distance(startingPosition, transform.position) < _dashDistance - 0.2f)
        {
            SceneReferencer.Instance.Player.SetImmune();
            if (!Dashing)
            {
                ResetToNonDashingState();
                return;
            }

            float t = Vector3.Distance(startingPosition, transform.position) / _dashDistance;
            float currentDashSpeed = Mathf.Lerp(0, _maxDashSpeed, _dashCurve.Evaluate(t));

            SetRigTransform(1 - t);
            _controller.SetVelocity(targetDirection * currentDashSpeed);
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
        }

        ResetToNonDashingState();
    }

    Plane _plane = new(Vector3.up, 0);

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

    private void ResetToNonDashingState()
    {
        SetRigTransform(0);
        Dashing = false;
        // _dashCollider.enabled = false;
        _lookAtMouse.SetEnabledState(true);
        LaughterManager.Instance.PlayLaughsByScore(DashScore).Forget();
        _dashTrail.SetActive(false);
        SceneReferencer.Instance.Player.SetImmune();
    }

    private async UniTask ChargeDash()
    {
        float passedTime = 0;

        AudioManager.Instance.Play("DashCharge");
        while (passedTime < _dashChargeTime)
        {
            float t = passedTime / _dashChargeTime;
            SetRigTransform(t);
            passedTime += Time.deltaTime;
            await UniTask.Yield();
        }

        SetRigTransform(1);
    }

    private void SetRigTransform(float t)
    {
        _rig.localPosition = new Vector3(0, Mathf.Lerp(0, _chargedDashYPosition, t), 0);
        _rig.localEulerAngles = new Vector3(Mathf.Lerp(0, _chargedDashXRotation, t), 0, 0);
    }
}