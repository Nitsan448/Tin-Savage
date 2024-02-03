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
        Arrow arrow = Instantiate(_arrowPrefab, transform.position, transform.rotation).GetComponent<Arrow>();
        arrow.Shoot(transform.forward);
    }


    private async UniTask KnockBack()
    {
        _characterController.RigidBody.ControlledPush(-transform.forward, 15, 100, GameConfiguration.Instance.PushCurve,
            _playerRigController.SetRigTransformAfterCrossbowShot, false).Forget();
    }
}