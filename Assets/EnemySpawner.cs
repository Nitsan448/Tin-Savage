using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _particleSystems;
    [SerializeField] private GameObject _enemy;

    [SerializeField] private float _scaleChangeDuration = 0.2f;
    [SerializeField] private float _timeBetweenParticlesAndEnemySpawn = 1;

    [SerializeField] private bool _useSpawnEffect;

    private void Start()
    {
        if (_useSpawnEffect)
        {
            SpawnEnemy().Forget();
        }
        else
        {
            _enemy.SetActive(true);
        }
    }

    private async UniTask SpawnEnemy()
    {
        //TODO: refactor this
        _enemy.SetActive(false);
        foreach (GameObject particleSystem in _particleSystems)
        {
            particleSystem.SetActive(true);
            particleSystem.transform.localScale = Vector3.zero;
            DOTween.To(() => particleSystem.transform.localScale, value => particleSystem.transform.localScale = value, Vector3.one,
                _scaleChangeDuration);
        }

        await UniTask.Delay(TimeSpan.FromSeconds(_scaleChangeDuration));
        await UniTask.Delay(TimeSpan.FromSeconds(_timeBetweenParticlesAndEnemySpawn));

        foreach (GameObject particleSystem in _particleSystems)
        {
            particleSystem.gameObject.SetActive(true);
            particleSystem.transform.localScale = Vector3.one;
            DOTween.To(() => particleSystem.transform.localScale, value => particleSystem.transform.localScale = value, Vector3.zero,
                _scaleChangeDuration);
        }

        _enemy.SetActive(true);
        _enemy.transform.localScale = Vector3.zero;
        DOTween.To(() => _enemy.transform.localScale, value => _enemy.transform.localScale = value, Vector3.one,
            _scaleChangeDuration);

        await UniTask.Delay(TimeSpan.FromSeconds(_scaleChangeDuration));
        foreach (GameObject particleSystem in _particleSystems)
        {
            particleSystem.gameObject.SetActive(false);
        }
    }
}