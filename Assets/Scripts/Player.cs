using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using CharacterController = Platformer3D.CharacterController;

public class Player : MonoBehaviour
{
    private CharacterController _controller;
    private LookAtMouse _lookAtMouse;
    private Dasher _dasher;
    private KeyManager _keyManager;
    private Knocker _knocker;

    private void Awake()
    {
        _dasher = GetComponent<Dasher>();
        _controller = GetComponent<CharacterController>();
        _keyManager = GetComponent<KeyManager>();
        _knocker = GetComponent<Knocker>();
    }

    private void Update()
    {
        if (_dasher.Dashing || _knocker.BeingKnocked) return;
        _controller.UpdateInput();
        if (Input.GetMouseButtonDown(0) && GameManager.Instance.State == EGameState.Running && _keyManager.HoldingKey)
        {
            _dasher.Dash().Forget();
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.State == EGameState.Running && !_dasher.Dashing && !_knocker.BeingKnocked)
        {
            _controller.CalculateVelocity();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Key key))
        {
            _keyManager.PickUpKey(key);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            if (_dasher.Dashing)
            {
                enemy.Die();
            }
            else if (!_knocker.BeingKnocked)
            {
                _knocker.Knock(enemy.transform.position, enemy.KnockBackDistance);
            }
        }
    }

    public void Die()
    {
        LevelLoader.Instance.LoadCurrentLevel();
    }
}