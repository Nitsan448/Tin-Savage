using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Dasher : MonoBehaviour
{
    [HideInInspector] public bool Dashing;
    [HideInInspector] public int DashScore;

    [SerializeField] private float _dashDistance;
    [SerializeField] private float _maxDashSpeed;
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

    public async UniTask Dash(CancellationTokenSource dashCts)
    {
        try
        {
            DoBeforeDashCharge();
            await ChargeDash(dashCts.Token);
            DoBeforeDash();
            await _controller.RigidBody.ControlledPush(transform.forward, _dashDistance, _maxDashSpeed,
                GameConfiguration.Instance.PushCurve,
                _playerRigController.SetRigTransformDuringDash, cancellationToken: dashCts.Token);
        }
        catch (OperationCanceledException)
        {
            DoOnDashCanceled();
        }

        DoAfterDash();
    }


    private void DoBeforeDashCharge()
    {
        Dashing = true;
        DashScore = 0;
        _keyManager.PlayKeyChargeAnimation();
        SceneReferencer.Instance.Player.SetImmune();
        // _playerKnocker.BeingKnocked = false;
        transform.rotation = transform.GetRotationTowardsOnYAxis(SceneReferencer.Instance.Player.GetMousePosition());
        _controller.RigidBody.isKinematic = true;
        AudioManager.Instance.Play("DashCharge");
    }

    private async UniTask ChargeDash(CancellationToken cancellationToken)
    {
        float passedTime = 0;

        while (passedTime < _dashChargeTime)
        {
            float t = passedTime / _dashChargeTime;
            _playerRigController.SetRigTransformDuringDashCharge(t);
            passedTime += Time.deltaTime;
            await UniTask.Yield(cancellationToken);
        }

        _playerRigController.SetRigTransformDuringDashCharge(1);
    }

    private void DoBeforeDash()
    {
        _dashTrail.SetActive(true);
        _controller.RigidBody.isKinematic = false;
        AudioManager.Instance.Play("Dash");
        if (!GameConfiguration.Instance.InfiniteDashes)
        {
            _keyManager.DropKey();
        }
    }

    private void DoAfterDash()
    {
        Dashing = false;
        CrowdManager.Instance.PlayLaughsByScore(DashScore).Forget();
        _dashTrail.SetActive(false);
        SceneReferencer.Instance.Player.SetImmune();
    }

    private void DoOnDashCanceled()
    {
        _playerRigController.SetRigTransformDuringDashCharge(0);
    }
}