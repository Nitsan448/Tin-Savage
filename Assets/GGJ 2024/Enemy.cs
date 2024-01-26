using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField, Range(0, 20)] private float _movementSpeed = 4f;
    [SerializeField] private GameObject _deathParticleSystemPrefab;
    [SerializeField] private bool _moveTowardsPlayer;

    private void FixedUpdate()
    {
        if (!_moveTowardsPlayer) return;
        Vector3 direction = SceneReferencer.Instance.Player.transform.position - transform.position;
        direction.Normalize();
        transform.position += direction * _movementSpeed * Time.deltaTime;
    }

    public void Die()
    {
        if (_deathParticleSystemPrefab != null)
        {
            Instantiate(_deathParticleSystemPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}