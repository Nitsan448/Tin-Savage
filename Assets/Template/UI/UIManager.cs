using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : ASingleton<UIManager>
{
    [Header("UI panels")] [SerializeField] private GameObject _mainPanel;

    [SerializeField] private GameObject _pausePanel;

    [SerializeField] private GameObject _optionsPanel;

    [Header("Overlay")] [SerializeField] private Image _overlay;

    [Header("Buttons")] [SerializeField] Button _backToMenuButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] Button _resumeButton;
    [SerializeField] Button _pauseRestartButton;
    [SerializeField] Button _winRestartButton;
    public GameObject WinPanel;

    protected override void DoOnAwake()
    {
        // SetPanelsAlpha();
        SetButtonEvents();
    }

    [ContextMenu("Set Panels Alpha")]
    private void SetPanelsAlpha()
    {
        _mainPanel.GetComponent<CanvasGroup>().alpha = 1;
        _pausePanel.GetComponent<CanvasGroup>().alpha = 0;
        _optionsPanel.GetComponent<CanvasGroup>().alpha = 0;
        _overlay.color = new Color(_overlay.color.r, _overlay.color.g, _overlay.color.b, 0);
    }

    private void SetButtonEvents()
    {
        _backToMenuButton.onClick.AddListener(BackToMainMenu);
        _resumeButton.onClick.AddListener(delegate { GameManager.Instance.TogglePauseState(); });
        _quitButton.onClick.AddListener(Application.Quit);
        _pauseRestartButton.onClick.AddListener(() =>
        {
            ChangePanelState(_pausePanel);
            GameManager.Instance.State = EGameState.Running;
            Time.timeScale = 1;
            LevelLoader.Instance.LoadCurrentLevel().Forget();
        });
        _winRestartButton.onClick.AddListener(() =>
        {
            WinPanel.SetActive(false);
            GameManager.Instance.State = EGameState.Running;
            Time.timeScale = 1;
            LevelLoader.Instance.LoadCurrentLevel().Forget();
        });
    }

    private void OnDestroy()
    {
        Fader.StopAllFades();
    }

    private void ChangePanelState(GameObject panel)
    {
        Fader.StopAllFades();
        if (!panel.activeSelf)
        {
            _mainPanel.GetComponent<CanvasGroup>().FadeOut();

            float overlayTargetAlpha = 150f / 255f;
            _overlay.Fade(overlayTargetAlpha).Forget();

            panel.GetComponent<CanvasGroup>().FadeIn();
        }
        else
        {
            panel.GetComponent<CanvasGroup>().FadeOut();
            _overlay.Fade(0).Forget();

            _mainPanel.GetComponent<CanvasGroup>().FadeIn();
        }
    }

    public void UpdatePauseMenuState()
    {
        ChangePanelState(_pausePanel);
    }

    public void BackToMainMenu()
    {
        LevelLoader.Instance.BackToMainMenu();
    }

    public void Quit()
    {
        Application.Quit();
    }
}