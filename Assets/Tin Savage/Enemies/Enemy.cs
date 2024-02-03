using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

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
    [SerializeField] private float _maxKnockBackSpeed = 20;
    [SerializeField] private float _knockBackDistance = 20;

    private Quaternion _rotationOnPush;
    [HideInInspector] public bool BeingKnockedBack;

    private void Awake()
    {
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

    public bool Hit(Transform hittingObject, int damage = 1)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Die();
            return true;
        }
        else
        {
            KnockBack(hittingObject).Forget();
            DoOnHit();
            if (_deathParticleSystemPrefab != null)
            {
                Instantiate(_deathParticleSystemPrefab, transform.position + _particleSystemSpawnOffset, Quaternion.identity);
            }

            return false;
        }
    }

    private async UniTask KnockBack(Transform hittingObject)
    {
        if (BeingKnockedBack)
        {
            return;
        }

        BeingKnockedBack = true;
        _rotationOnPush = transform.rotation;
        Vector3 hittingObjectPosition = new(hittingObject.position.x, transform.position.y, hittingObject.position.z);
        // Vector3 targetDirection =
        //     new Vector3((transform.position - hittingObjectPosition).x, 0, (transform.position - hittingObjectPosition).z)
        //         .normalized;
        Vector3 targetDirection = -transform.forward;
        targetDirection.Normalize();
        await _rigidbody.ControlledPush(targetDirection, _knockBackDistance, _maxKnockBackSpeed, GameConfiguration.Instance.PushCurve,
            UpdateRotationOnPushProgress);
        BeingKnockedBack = false;
    }

    private void UpdateRotationOnPushProgress(float t)
    {
        transform.rotation =
            Quaternion.Lerp(_rotationOnPush,
                Quaternion.Euler(_rotationOnPush.eulerAngles.x, _rotationOnPush.eulerAngles.y + 360, _rotationOnPush.eulerAngles.z), t);
    }

    protected virtual void DoOnHit()
    {
    }

    public void Die()
    {
        if (_deathParticleSystemPrefab != null)
        {
            Instantiate(_deathParticleSystemPrefab, transform.position + _particleSystemSpawnOffset, Quaternion.identity);
        }

        AudioManager.Instance.Play(_deathSoundName);
        DoOnDeath();
        Destroy(gameObject);
    }

    protected virtual void DoOnDeath()
    {
    }
}