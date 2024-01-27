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
    [SerializeField] protected int _health;
    private EnemyKnocker _enemyKnocker;
    private Wave _wave;

    public void Init(Wave wave)
    {
        _wave = wave;
        _enemyKnocker = GetComponent<EnemyKnocker>();
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

    protected void DoOnHit()
    {
        if (_enemyKnocker != null)
        {
            _enemyKnocker.Knock(SceneReferencer.Instance.Player.transform.position).Forget();
        }
    }

    public void Die()
    {
        Debug.Log("here");
        if (_deathParticleSystemPrefab != null)
        {
            Instantiate(_deathParticleSystemPrefab, transform.position, Quaternion.identity);
        }

        _wave.OnEnemyDeath();
        Destroy(gameObject);
    }
}