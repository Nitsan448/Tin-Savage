using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class AWeapon : MonoBehaviour
{
    protected PlayerRigController _playerRigController;
    protected CharacterController _characterController;
    [HideInInspector] public bool Shooting;


    public void Init(PlayerRigController playerRigController, CharacterController controller)
    {
        _playerRigController = playerRigController;
        _characterController = controller;
    }

    public virtual async UniTask Shoot()
    {
    }
}