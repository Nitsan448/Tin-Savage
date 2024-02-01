using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Crossbow : AWeapon
{
    [SerializeField] private float _shotChargeTime;
    [SerializeField] private GameObject _arrowPrefab;

    public override void Shoot()
    {
        Debug.Log("here");
        ShootAsync().Forget();
    }

    private async UniTask ShootAsync()
    {
        Shooting = true;
        transform.rotation = transform.GetRotationTowardsOnYAxis(SceneReferencer.Instance.Player.GetMousePosition());
        _characterController.RigidBody.isKinematic = true;
        await ChargeShot();
        ShootArrow();
        _characterController.RigidBody.isKinematic = false;

        Shooting = false;
    }

    private async UniTask ChargeShot(CancellationToken cancellationToken = default)
    {
        float passedTime = 0;
        while (passedTime < _shotChargeTime)
        {
            float t = passedTime / _shotChargeTime;
            _playerRigController.SetRigTransformDuringCrossbowShot(t);
            passedTime += Time.deltaTime;
            await UniTask.Yield(cancellationToken);
        }

        _playerRigController.SetRigTransformDuringCrossbowShot(1);
    }

    private void ShootArrow()
    {
        Arrow arrow = Instantiate(_arrowPrefab, transform.position, transform.rotation).GetComponent<Arrow>();
        arrow.Shoot(transform.forward);
        ReturnRigToRegularPosition().Forget();
    }

    private async UniTask ReturnRigToRegularPosition()
    {
        float currentTime = 0;
        while (currentTime < 0.15f)
        {
            float t = currentTime / 0.1f;
            _playerRigController.SetRigTransformDuringCrossbowShot(1 - t);
            currentTime += Time.deltaTime;
            await UniTask.Yield();
        }

        _playerRigController.SetRigTransformDuringCrossbowShot(0);
    }
}