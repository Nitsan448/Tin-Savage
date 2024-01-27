using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<int> _timePerWave;
    [SerializeField] private List<GameObject> _possibleWavesEasyPrefabs;
    [SerializeField] private List<GameObject> _possibleWavesMediumPrefabs;
    [SerializeField] private List<GameObject> _possibleWavesHardPrefabs;
    private List<GameObject> _possibleWavesPrefabsHardStarting = new();

    [SerializeField] private int _mediumWavesStartIndex;
    [SerializeField] private int _hardWavesStartIndex;


    private int _enemiesLeftInWave;

    private void Start()
    {
        foreach (GameObject wave in _possibleWavesHardPrefabs)
        {
            _possibleWavesPrefabsHardStarting.Add(wave);
        }

        SpawnWaves().Forget();
    }

    private async UniTask SpawnWaves()
    {
        for (int waveTimeIndex = 0; waveTimeIndex < _timePerWave.Count; waveTimeIndex++)
        {
            int waveTime = _timePerWave[waveTimeIndex];
            if (_possibleWavesHardPrefabs.Count == 0)
            {
                _possibleWavesHardPrefabs = _possibleWavesPrefabsHardStarting;
            }

            List<GameObject> waves = _possibleWavesEasyPrefabs;
            if (waveTimeIndex > _mediumWavesStartIndex)
            {
                waves = _possibleWavesMediumPrefabs;
            }

            if (waveTimeIndex > _hardWavesStartIndex)
            {
                waves = _possibleWavesHardPrefabs;
            }

            int waveIndex = Random.Range(0, waves.Count);
            Instantiate(waves[waveIndex], Vector3.zero, Quaternion.identity);
            waves.RemoveAt(waveIndex);
            await UniTask.Delay(TimeSpan.FromSeconds(waveTime));
        }
    }
}