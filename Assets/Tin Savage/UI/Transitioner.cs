using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Transitioner : ASingleton<Transitioner>
{
    [SerializeField] private MeshRenderer _transitionPlane;

    [SerializeField] private float _gameOverFadeInTime = 2;
    [SerializeField] private float _startFadeInTime = 2;
    [SerializeField] private float _transitionPlaneIntensity;
    [SerializeField] private GameObject _gameOverTitle;
    [SerializeField] private GameObject _waveManager;

    private void Start()
    {
        FadeInToLevel().Forget();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            LevelLoader.Instance.LoadCurrentLevel().Forget();
        }
    }

    public async UniTask GameOver()
    {
        float currentTime = 0;
        _gameOverTitle.SetActive(true);
        Material transitionPlaneMaterial = _transitionPlane.material;
        while (currentTime < _gameOverFadeInTime)
        {
            float t = currentTime / _gameOverFadeInTime;
            transitionPlaneMaterial.SetFloat("_Transition", Mathf.Lerp(0, _transitionPlaneIntensity, t));
            currentTime += Time.deltaTime;
            await UniTask.Yield();
        }

        transitionPlaneMaterial.SetFloat("_Transition", _transitionPlaneIntensity);

        while (true)
        {
            if (Input.anyKey)
            {
                LevelLoader.Instance.LoadCurrentLevel().Forget();
                return;
            }

            await UniTask.Yield();
        }
    }

    public async UniTask FadeInToLevel()
    {
        float currentTime = 0;
        Material transitionPlaneMaterial = _transitionPlane.material;
        while (currentTime < _startFadeInTime)
        {
            float t = currentTime / _startFadeInTime;
            transitionPlaneMaterial.SetFloat("_Transition", Mathf.Lerp(1, 0, t));
            currentTime += Time.deltaTime;
            await UniTask.Yield();
        }

        transitionPlaneMaterial.SetFloat("_Transition", 0);
    }

    public async UniTask Win()
    {
        GameManager.Instance.State = EGameState.Finished;
        float currentTime = 0;
        Material transitionPlaneMaterial = _transitionPlane.material;
        UIManager.Instance.WinPanel.gameObject.SetActive(true);

        // foreach (Enemy enemy in FindObjectsOfType<Enemy>())
        // {
        //     Debug.Log(enemy.name);
        // }
        //
        Destroy(_waveManager);
        //

        while (currentTime < _gameOverFadeInTime)
        {
            float t = currentTime / _gameOverFadeInTime;
            transitionPlaneMaterial.SetFloat("_Transition", Mathf.Lerp(0, _transitionPlaneIntensity, t));
            currentTime += Time.deltaTime;
            await UniTask.Yield();
        }

        transitionPlaneMaterial.SetFloat("_Transition", _transitionPlaneIntensity);
    }
}