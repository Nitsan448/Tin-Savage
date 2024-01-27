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
    private Wave _wave;
    [SerializeField] private Vector3 _particleSystemSpawnOffset;
    public bool KillPlayerOnHit;
    [SerializeField] private string _deathSoundName;
    [SerializeField] public int Score;
    [SerializeField] private MeshRenderer _mesh;

    private void Awake()
    {
        _enemyKnocker = GetComponent<EnemyKnocker>();
    }

    private void Start()
    {
        AudioManager.Instance.Play("EnemySpawn");
    }

    public void Init(Wave wave)
    {
        _wave = wave;
    }

    public bool Hit()
    {
        _health--;
        if (_health <= 0)
        {
            Die();
            return true;
        }
        else
        {
            DoOnHit();
            return false;
        }
    }

    protected virtual void DoOnHit()
    {
        if (_enemyKnocker != null)
        {
            _enemyKnocker.Knock(SceneReferencer.Instance.Player.transform.position).Forget();
        }
    }

    public async UniTask Die(bool diedFromDash = true)
    {
        if (_deathParticleSystemPrefab != null)
        {
            Instantiate(_deathParticleSystemPrefab, transform.position + _particleSystemSpawnOffset, Quaternion.identity);
        }

        AudioManager.Instance.Play(_deathSoundName);
        if (!diedFromDash)
        {
            LaughterManager.Instance.PlayLaughsByScore(Score).Forget();
        }

        // _wave.OnEnemyDeath();
        Destroy(gameObject);
    }
}