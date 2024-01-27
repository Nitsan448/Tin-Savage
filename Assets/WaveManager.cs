using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<int> _timePerWave;
    [SerializeField] private List<GameObject> _possibleWavesPrefabs;
    private List<GameObject> _possibleWavesPrefabsStarting = new();


    private int _enemiesLeftInWave;

    private void Start()
    {
        foreach (GameObject wave in _possibleWavesPrefabs)
        {
            _possibleWavesPrefabsStarting.Add(wave);
        }

        SpawnWaves().Forget();
    }

    private async UniTask SpawnWaves()
    {
        foreach (int waveTime in _timePerWave)
        {
            if (_possibleWavesPrefabs.Count == 0)
            {
                _possibleWavesPrefabs = _possibleWavesPrefabsStarting;
            }

            int waveIndex = Random.Range(0, _possibleWavesPrefabs.Count);
            Instantiate(_possibleWavesPrefabs[waveIndex], Vector3.zero, Quaternion.identity);
            _possibleWavesPrefabs.RemoveAt(waveIndex);
            await UniTask.Delay(TimeSpan.FromSeconds(waveTime));
        }
    }
}