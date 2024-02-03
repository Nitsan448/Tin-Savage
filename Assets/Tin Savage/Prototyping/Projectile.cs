using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed = 5;
    [SerializeField] protected int _damage = 1;
    [SerializeField] private float _lifeTime = 10;
    private Rigidbody _rigidbody;
    private CancellationTokenSource _shootCts = new();

    private void OnDestroy()
    {
        _shootCts.Cancel();
    }

    public virtual async UniTask Shoot(Vector3 direction)
    {
        _rigidbody = GetComponent<Rigidbody>();
        float _currentTime = 0;
        while (_currentTime < _lifeTime)
        {
            _rigidbody.velocity = direction * _speed;
            _currentTime += Time.deltaTime;
            await UniTask.Yield(cancellationToken: _shootCts.Token);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            Debug.Log(enemy.name);
            //TODO: refactor enemy hit return value
            if (!enemy.Hit(transform, _damage))
            {
                Destroy(gameObject);
            }
        }
    }
}