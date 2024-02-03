using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _lifeTime = 10;
    private Rigidbody _rigidbody;

    public virtual async UniTask Shoot(Vector3 direction)
    {
        _rigidbody = GetComponent<Rigidbody>();
        float _currentTime = 0;
        while (_currentTime < _lifeTime)
        {
            _rigidbody.velocity = direction * _speed;
            _currentTime += Time.deltaTime;
            await UniTask.Yield();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.Die();
            Destroy(gameObject);
        }
    }
}