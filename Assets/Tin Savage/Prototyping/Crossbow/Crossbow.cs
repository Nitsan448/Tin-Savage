using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Crossbow : AWeapon
{
    [SerializeField] private float _shotChargeTime;
    [SerializeField] private GameObject _arrowPrefab;

    public override async UniTask Shoot()
    {
        Shooting = true;
        transform.rotation = transform.GetRotationTowardsOnYAxis(SceneReferencer.Instance.Player.GetMousePosition());

        //TODO: find another way without setting iskinematic
        // _characterController.RigidBody.isKinematic = true;
        _characterController.SetVelocity(Vector3.zero);
        await ChargeShot();
        ShootArrow();
        // _characterController.RigidBody.isKinematic = false;
        await KnockBack();
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
        Projectile arrow = Instantiate(_arrowPrefab, transform.position, transform.rotation).GetComponent<Projectile>();
        arrow.Shoot(transform.forward).Forget();
    }


    private async UniTask KnockBack()
    {
        await _characterController.RigidBody.ControlledPush(-transform.forward, 15, 100, GameConfiguration.Instance.PushCurve,
            _playerRigController.SetRigTransformAfterCrossbowShot);
    }
}