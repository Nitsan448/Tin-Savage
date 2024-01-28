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
    [SerializeField] private GameObject _bossWave;

    private readonly List<GameObject> _possibleWavesPrefabsEasyStarting = new();

    [SerializeField] private int _mediumWavesStartIndex;
    [SerializeField] private int _hardWavesStartIndex;


    private int _enemiesLeftInWave;

    private void Start()
    {
        foreach (GameObject wave in _possibleWavesEasyPrefabs)
        {
            _possibleWavesPrefabsEasyStarting.Add(wave);
        }

        SpawnWaves().Forget();
    }


    private async UniTask SpawnWaves()
    {
        await PlayTutorial();
        for (int waveTimeIndex = GameConfiguration.Instance.StartingWave; waveTimeIndex < _timePerWave.Count; waveTimeIndex++)
        {
            int waveTime = _timePerWave[waveTimeIndex];
            int waveIndex;

            List<GameObject> waves = _possibleWavesEasyPrefabs;
            if (waveTimeIndex > _mediumWavesStartIndex)
            {
                waves = _possibleWavesMediumPrefabs;
            }

            if (waveTimeIndex > _hardWavesStartIndex)
            {
                waves = _possibleWavesHardPrefabs;
            }

            waveIndex = Random.Range(0, waves.Count);
            Instantiate(waves[waveIndex], Vector3.zero, Quaternion.identity, parent: transform);
            waves.RemoveAt(waveIndex);
            await UniTask.Delay(TimeSpan.FromSeconds(waveTime));
        }

        Instantiate(_bossWave, Vector3.zero, Quaternion.identity, parent: transform);
        while (GameManager.Instance.State == EGameState.Running)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(8));
            int waveIndex = Random.Range(0, _possibleWavesPrefabsEasyStarting.Count);
            Instantiate(_possibleWavesPrefabsEasyStarting[waveIndex], Vector3.zero, Quaternion.identity, parent: transform);
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