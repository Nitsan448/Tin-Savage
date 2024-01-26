using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject _deathParticleSystemPrefab;

    public void Die()
    {
        if (_deathParticleSystemPrefab != null)
        {
            Instantiate(_deathParticleSystemPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}