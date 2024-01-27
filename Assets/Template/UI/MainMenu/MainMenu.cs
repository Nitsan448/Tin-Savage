using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _mainPanel;

    [SerializeField] private GameObject _optionsPanel;

    [SerializeField] private Button _playButton;

    [SerializeField] private Button _optionsButton;

    [SerializeField] private Button _exitButton;

    [SerializeField] private Button _backToMainPanelButton;

    private void Awake()
    {
        //TODO: change these to methods
        _exitButton.onClick.AddListener(
            Application.Quit
        );

        _playButton.onClick.AddListener(
            delegate
            {
                SceneManager.LoadScene("BaseScene");
                SceneManager.LoadSceneAsync("ARENA", LoadSceneMode.Additive);
            }
        );

        _optionsButton.onClick.AddListener(
            delegate
            {
                _optionsPanel.SetActive(true);
                _mainPanel.SetActive(false);
            }
        );

        _backToMainPanelButton.onClick.AddListener(
            delegate
            {
                _mainPanel.SetActive(true);
                _optionsPanel.SetActive(false);
            }
        );
    }
}