using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private float _timeUntilPlayPossible;
    private bool _playPossible;

    private void Start()
    {
        MainMenu().Forget();
    }

    private async UniTask MainMenu()
    {
        float passedTime = 0;
        while (true)
        {
            if (Input.anyKey && passedTime > 8.5f)
            {
                SceneManager.LoadScene("BaseScene");
                SceneManager.LoadScene("ARENA", LoadSceneMode.Additive);
                return;
            }

            passedTime += Time.deltaTime;

            await UniTask.Yield();
        }
    }
}