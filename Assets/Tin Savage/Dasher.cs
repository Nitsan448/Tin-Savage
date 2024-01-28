using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Dasher : MonoBehaviour
{
    public bool Dashing;
    public int DashScore;

    [SerializeField] private float _dashDistance;
    [SerializeField] private float _maxDashSpeed;
    [SerializeField] private AnimationCurve _dashCurve;
    [SerializeField] private float _dashChargeTime;
    [SerializeField] private GameObject _dashTrail;

    private CharacterController _controller;
    private KeyManager _keyManager;
    private PlayerRigController _playerRigController;

    public void Init(CharacterController controller, KeyManager keyManager, PlayerRigController playerRigController)
    {
        _controller = controller;
        _keyManager = keyManager;
        _playerRigController = playerRigController;
    }

    public async UniTask Dash()
    {
        DoBeforeDashCharge();
        await ChargeDash(_dashChargeTime);
        DoBeforeDash();
        await _controller.RigidBody.ControlledPush(transform.forward, _dashDistance, _maxDashSpeed, _dashCurve,
            _playerRigController.SetRigTransformDuringDash);
        DoAfterDash();
    }


    private void DoBeforeDashCharge()
    {
        DashScore = 0;
        _keyManager.KeyAnimator.SetTrigger("Charge");
        _controller.SetVelocity(Vector3.zero);
        Dashing = true;
        SceneReferencer.Instance.Player.SetImmune();
        // _playerKnocker.BeingKnocked = false;
        _controller.RigidBody.isKinematic = true;
        _dashTrail.SetActive(true);
        AudioManager.Instance.Play("DashCharge");
    }

    private async UniTask ChargeDash(float chargeTime)
    {
        float passedTime = 0;

        while (passedTime < chargeTime)
        {
            float t = passedTime / chargeTime;
            _playerRigController.SetRigTransformDuringDashCharge(t);
            passedTime += Time.deltaTime;
            await UniTask.Yield();
        }

        _playerRigController.SetRigTransformDuringDashCharge(1);
    }

    private void DoBeforeDash()
    {
        _controller.RigidBody.isKinematic = false;
        AudioManager.Instance.Play("Dash");
        _keyManager.DropKey();
        transform.rotation = transform.GetRotationTowardsOnYAxis(SceneReferencer.Instance.Player.GetMousePosition());
    }

    private void DoAfterDash()
    {
        Dashing = false;
        CrowdManager.Instance.PlayLaughsByScore(DashScore).Forget();
        _dashTrail.SetActive(false);
        SceneReferencer.Instance.Player.SetImmune();
    }
}