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
    private readonly List<GameObject> _possibleWavesPrefabsHardStarting = new();

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
        await PlayTutorial();
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
            Instantiate(waves[waveIndex], Vector3.zero, Quaternion.identity, parent: transform);
            waves.RemoveAt(waveIndex);
            await UniTask.Delay(TimeSpan.FromSeconds(waveTime));
        }
    }

    private async UniTask PlayTutorial()
    {
        while (SceneReferencer.Instance.Player.InTutorial)
        {
            await UniTask.Yield();
        }
    }
}