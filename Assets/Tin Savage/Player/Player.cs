using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController _controller;
    private Dasher _dasher;
    private KeyManager _keyManager;
    private PlayerKnocker _playerKnocker;
    [SerializeField, Range(0, 3)] private float _immunityTimeAfterDash;
    private AudioSource _playerWalkSound;

    private bool _immune;
    private float _timeSinceLastDashFinished = 0;
    public bool InTutorial = true;
    private Plane _mouseRayCastPlane = new(Vector3.up, 0);
    [SerializeField] private GameObject _dashTrail;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _dashChargeTime;
    private PlayerRigController _playerRigController;
    private CancellationTokenSource _dashCts;

    private void Awake()
    {
        _playerWalkSound = GetComponent<AudioSource>();
        _dasher = GetComponent<Dasher>();
        _controller = GetComponent<CharacterController>();
        _keyManager = GetComponent<KeyManager>();
        _playerKnocker = GetComponent<PlayerKnocker>();
        _playerRigController = GetComponent<PlayerRigController>();
        _dasher.Init(_controller, _keyManager, _playerRigController);
    }

    private void Update()
    {
        if (_dasher.Dashing || _playerKnocker.BeingKnocked || GameManager.Instance.State != EGameState.Running) return;

        _controller.UpdateInput();

        if (Input.GetMouseButtonDown(0) && _keyManager.HoldingKey)
        {
            _dashCts = new CancellationTokenSource();
            _dasher.Dash(_dashCts).Forget();
        }

        UpdateImmuneState();
    }

    private void UpdateImmuneState()
    {
        if (!_immune || _dasher.Dashing) return;

        _timeSinceLastDashFinished += Time.deltaTime;
        if (_timeSinceLastDashFinished > _immunityTimeAfterDash)
        {
            _immune = false;
        }
    }

    private void FixedUpdate()
    {
        SetPlayerWalkSoundPlayingState();
        if (GameManager.Instance.State != EGameState.Running || _dasher.Dashing || _playerKnocker.BeingKnocked) return;

        _controller.CalculateVelocity();
        transform.RotateTowardsOnYAxis(SceneReferencer.Instance.Player.GetMousePosition(), _rotationSpeed);
    }

    private void SetPlayerWalkSoundPlayingState()
    {
        if (!_playerWalkSound.isPlaying && _controller.RigidBody.velocity.magnitude > 1)
        {
            _playerWalkSound.Play();
        }
        else if (_playerWalkSound.isPlaying &&
                 (_controller.RigidBody.velocity.magnitude < 1 || _playerKnocker.BeingKnocked || _dasher.Dashing))
        {
            _playerWalkSound.Stop();
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
        if (!other.gameObject.TryGetComponent(out Enemy enemy)) return;
        if (_dasher.Dashing)
        {
            HitEnemyWithDash(enemy);
            return;
        }

        if (_immune) return;

        if (enemy.KillPlayerOnHit)
        {
            Die();
        }
        else
        {
            if (_dasher.Dashing)
            {
                _dashCts.Cancel();
            }

            _playerWalkSound.Stop();
            _playerKnocker.BeingKnocked = false;
            _playerKnocker.Knock(enemy.transform.position, enemy.KnockPlayerDistance).Forget();
        }
    }


    private void HitEnemyWithDash(Enemy enemy)
    {
        _dasher.DashScore += enemy.Score;
        bool enemyDied = enemy.Hit(this);
        if (enemyDied) return;

        _dashCts.Cancel();
        _playerWalkSound.Stop();
        _playerKnocker.Knock(transform.position + transform.forward, 1).Forget();
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
        CrowdManager.Instance.PlayLaughsByScore(6).Forget();
        Transitioner.Instance.GameOverAsync().Forget();
    }

    public Vector3 GetMousePosition()
    {
        _mouseRayCastPlane.distance = -transform.position.y;
        if (Camera.main == null) return Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return _mouseRayCastPlane.Raycast(ray, out float distance) ? ray.GetPoint(distance) : Vector3.zero;
    }
}