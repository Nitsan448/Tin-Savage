using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : ASingleton<GameManager>
{
    public EGameState State = EGameState.Running;

    private void Update()
    {
        //TODO: move to input manager
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseState();
        }
    }

    public void TogglePauseState()
    {
        if (State == EGameState.Finished)
        {
            return;
        }

        State = State == EGameState.Running ? EGameState.Paused : EGameState.Running;
        if (State == EGameState.Paused)
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1;
        }

        UIManager.Instance.UpdatePauseMenuState();
    }
}