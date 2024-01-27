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
        while (true)
        {
            if (Input.anyKey)
            {
                SceneManager.LoadScene("BaseScene");
                SceneManager.LoadScene("ARENA", LoadSceneMode.Additive);
                return;
            }

            await UniTask.Yield();
        }
    }
}