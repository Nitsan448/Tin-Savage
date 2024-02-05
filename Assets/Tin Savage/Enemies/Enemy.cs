using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Enemy : MonoBehaviour
{
    public int KnockPlayerDistance = 20;
    [SerializeField] private GameObject _deathParticleSystemPrefab;

    [SerializeField] protected int _health = 1;

    [SerializeField] private Vector3 _particleSystemSpawnOffset;
    public bool KillPlayerOnHit;
    [SerializeField] private string _deathSoundName;
    [SerializeField] public int Score;
    private Rigidbody _rigidbody;

    [FormerlySerializedAs("_maxKnockBackSpeed")] [SerializeField]
    private float _maxKnockSpeed = 20;

    [FormerlySerializedAs("_knockBackDistance")] [SerializeField]
    private float _knockDistance = 20;

    private Quaternion _rotationOnPush;
    [HideInInspector] public bool BeingKnocked;
    private CancellationTokenSource _enemyCts = new();
    [SerializeField] private float _timeUntilDeadly = 1;
    public bool IsDeadly => _timeUntilDeadly < Time.time;

    private void Awake()
    {
        _timeUntilDeadly += Time.time;
        _rigidbody = GetComponent<Rigidbody>();
        DoOnAwake();
    }

    protected virtual void DoOnAwake()
    {
    }

    private void Start()
    {
        AudioManager.Instance.Play("EnemySpawn");
    }

    private void OnDestroy()
    {
        _enemyCts.Cancel();
        //TODO: refactor
        Destroy(transform.parent.gameObject);
    }

    public bool Hit(Vector3 hittingObjectDirection, int damage = 1)
    {
        _health -= damage;
        if (_deathParticleSystemPrefab != null)
        {
            Instantiate(_deathParticleSystemPrefab, transform.position + _particleSystemSpawnOffset, Quaternion.identity);
        }

        if (_health <= 0)
        {
            Die();
            return true;
        }
        else
        {
            DoOnHit();
            KnockBack(hittingObjectDirection).Forget();

            return false;
        }
    }

    private async UniTask KnockBack(Vector3 knockDirection)
    {
        if (BeingKnocked)
        {
            return;
        }

        gameObject.layer = LayerMask.NameToLayer("Default");
        BeingKnocked = true;
        await _rigidbody.ControlledPush(knockDirection.normalized, _knockDistance, _maxKnockSpeed, GameConfiguration.Instance.PushCurve,
            cancellationToken: _enemyCts.Token);
        BeingKnocked = false;
        gameObject.layer = LayerMask.NameToLayer("Agent");
    }

    protected virtual void DoOnHit()
    {
    }

    public void Die()
    {
        AudioManager.Instance.Play(_deathSoundName);
        DoOnDeath();
        Destroy(gameObject);
    }

    protected virtual void DoOnDeath()
    {
    }
}