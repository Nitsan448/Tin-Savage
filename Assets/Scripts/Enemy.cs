using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Enemy : MonoBehaviour
{
    public int KnockBackDistance = 30;
    [SerializeField] private GameObject _deathParticleSystemPrefab;
    private Wave _wave;

    public void Init(Wave wave)
    {
        _wave = wave;
    }

    public void Die()
    {
        if (_deathParticleSystemPrefab != null)
        {
            Instantiate(_deathParticleSystemPrefab, transform.position, Quaternion.identity);
        }

        _wave.OnEnemyDeath();
        Destroy(gameObject);
    }
}