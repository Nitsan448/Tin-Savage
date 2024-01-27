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
    private PlayerKnocker _playerKnocker;
    [SerializeField, Range(0, 3)] private float _immunityTimeAfterDash;
    private AudioSource _playerWalkSound;

    private bool _immune;
    private float _timeSinceLastDashFinished = 0;
    public bool InTutorial = true;

    private void Awake()
    {
        _playerWalkSound = GetComponent<AudioSource>();
        _dasher = GetComponent<Dasher>();
        _controller = GetComponent<CharacterController>();
        _keyManager = GetComponent<KeyManager>();
        _playerKnocker = GetComponent<PlayerKnocker>();
    }

    private void Update()
    {
        if (_dasher.Dashing || _playerKnocker.BeingKnocked) return;
        _controller.UpdateInput();
        if (Input.GetMouseButtonDown(0) && GameManager.Instance.State == EGameState.Running && _keyManager.HoldingKey)
        {
            _playerWalkSound.Stop();
            _dasher.Dash().Forget();
        }

        if (_immune)
        {
            _timeSinceLastDashFinished += Time.deltaTime;
            if (_timeSinceLastDashFinished > _immunityTimeAfterDash)
            {
                _immune = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.State == EGameState.Running && !_dasher.Dashing && !_playerKnocker.BeingKnocked)
        {
            if (!_playerWalkSound.isPlaying && _controller.RigidBody.velocity.magnitude > 1)
            {
                _playerWalkSound.Play();
            }
            else if (_playerWalkSound.isPlaying && _controller.RigidBody.velocity.magnitude < 1)
            {
                _playerWalkSound.Stop();
            }

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

    private void OnTriggerStay(Collider other)
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
                Debug.Log(enemy.name);
                _dasher.DashScore += enemy.Score;
                bool enemyDied = enemy.Hit();
                if (!enemyDied)
                {
                    _dasher.Dashing = false;
                    _playerWalkSound.Stop();
                    _playerKnocker.Knock(transform.position + transform.forward, 1).Forget();
                }
            }
            else if (!_immune)
            {
                if (enemy.KillPlayerOnHit)
                {
                    Die();
                }
                else
                {
                    _playerWalkSound.Stop();
                    _playerKnocker.BeingKnocked = false;
                    _playerKnocker.Knock(enemy.transform.position, enemy.KnockPlayerDistance).Forget();
                }
            }
        }
    }

    public void SetImmune()
    {
        _immune = true;
        _timeSinceLastDashFinished = 0;
    }

    public void Die()
    {
        AudioManager.Instance.Play("Death");
        _playerWalkSound.Stop();
        LaughterManager.Instance.PlayLaughsByScore(6).Forget();
        Transitioner.Instance.GameOverAsync().Forget();
    }
}