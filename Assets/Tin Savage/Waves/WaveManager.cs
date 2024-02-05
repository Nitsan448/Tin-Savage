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
    [SerializeField] private GameObject _tutorialWave;

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
        if (GameConfiguration.Instance.PlayTutorial)
        {
            await Tutorial.Instance.PlayTutorial();
        }

        for (int waveTimeIndex = GameConfiguration.Instance.StartingWave; waveTimeIndex < _timePerWave.Count; waveTimeIndex++)
        {
            float waveTime = _timePerWave[waveTimeIndex] / GameConfiguration.Instance.TimeBetweenWavesScale;
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
            await UniTask.Delay(TimeSpan.FromSeconds(7));
            int waveIndex = Random.Range(0, _possibleWavesPrefabsEasyStarting.Count);
            Instantiate(_possibleWavesPrefabsEasyStarting[waveIndex], Vector3.zero, Quaternion.identity, parent: transform);
        }
    }

    public async UniTask PlayTutorialWave()
    {
        GameObject tutorialWave = Instantiate(_tutorialWave, Vector3.zero, Quaternion.identity, parent: transform);
        Enemy spider = tutorialWave.GetComponentInChildren<Enemy>();
        await UniTask.WaitUntil(() => spider == null);
    }
}