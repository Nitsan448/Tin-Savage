using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameOver : ASingleton<GameOver>
{
    [SerializeField] private Material _transitionPlane;

    [SerializeField] private float _fadeInTime = 2;
    [SerializeField] private float _transitionPlaneIntensity;
    private GameObject _gameOverTitle;

    // Start is called before the first frame update
    void Start()
    {
    }

    private async UniTask GameOver()
    {
        float currentTime = 0;
        while (currentTime < _fadeInTime)
        {
            float t = currentTime / _fadeInTime;
            _transitionPlane.SetFloat("_Transition", Mathf.Lerp(0, _transitionPlaneIntensity, t));
            await UniTask.Yield();
        }

        while (true)
        {
            if (Input.anyKey)
            {
                
            }
            
            
        }
    }
}