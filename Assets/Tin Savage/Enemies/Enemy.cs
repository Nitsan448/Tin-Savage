using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(EnemyKnocker))]
public class Enemy : MonoBehaviour
{
    public int KnockPlayerDistance = 20;
    [SerializeField] private GameObject _deathParticleSystemPrefab;
    [SerializeField] protected int _health = 1;
    private EnemyKnocker _enemyKnocker;
    [SerializeField] private Vector3 _particleSystemSpawnOffset;
    public bool KillPlayerOnHit;
    [SerializeField] private string _deathSoundName;
    [SerializeField] public int Score;

    private void Awake()
    {
        _enemyKnocker = GetComponent<EnemyKnocker>();
        DoOnAwake();
    }

    protected virtual void DoOnAwake()
    {
    }

    private void Start()
    {
        AudioManager.Instance.Play("EnemySpawn");
    }

    public bool Hit()
    {
        _health--;
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
            if (_enemyKnocker != null)
            {
                _enemyKnocker.Knock(SceneReferencer.Instance.Player.transform.position).Forget();
            }

            DoOnHit();
            return false;
        }
    }

    protected virtual void DoOnHit()
    {
    }

    public async UniTask Die(bool diedFromDash = true)
    {
        AudioManager.Instance.Play(_deathSoundName);
        if (!diedFromDash)
        {
            CrowdManager.Instance.PlayLaughsByScore(Score).Forget();
        }

        DoOnDeath();
        // _wave.OnEnemyDeath();
        Destroy(gameObject);
    }

    protected virtual void DoOnDeath()
    {
    }
}