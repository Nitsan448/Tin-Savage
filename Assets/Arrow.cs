using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float _speed = 5;
    private Rigidbody _rigidbody;

    public async UniTask Shoot(Vector3 direction)
    {
        _rigidbody = GetComponent<Rigidbody>();
        float _currentTime = 0;
        while (_currentTime < 10)
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
            Debug.Log("hit enemy");
            enemy.Die();
            Destroy(gameObject);
        }
    }
}