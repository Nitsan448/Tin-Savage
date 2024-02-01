using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AWeapon : MonoBehaviour
{
    [SerializeField] protected int _damage;
    protected PlayerRigController _playerRigController;
    protected CharacterController _characterController;
    public bool Shooting;


    public void Init(PlayerRigController playerRigController, CharacterController controller)
    {
        _playerRigController = playerRigController;
        _characterController = controller;
    }

    public abstract void Shoot();
}