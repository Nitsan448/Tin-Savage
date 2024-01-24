using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : ASingleton<LevelLoader>
{
    public string CurrentLevel => _currentLevel;

    private string _currentLevel = "Level1";

    [PropertySpace(5)]
    [Button]
    public void LoadNextLevel()
    {
        SceneManager.UnloadSceneAsync(_currentLevel);

        int numericPartStartIndex = _currentLevel.GetNumericPartStartIndex();

        string numericPart = numericPartStartIndex != -1 ? _currentLevel[numericPartStartIndex..] : "";

        bool levelFormatIsValid = int.TryParse(numericPart, out int levelNumber);
        if (levelFormatIsValid)
        {
            _currentLevel = _currentLevel.Replace(numericPart, (levelNumber + 1).ToString());
        }

        SceneManager.LoadScene(_currentLevel, LoadSceneMode.Additive);
    }

    [PropertySpace(5)]
    [Button]
    public void LoadLevelByNumber(int level)
    {
        SceneManager.UnloadSceneAsync(_currentLevel);
        _currentLevel = "Level" + level;
        SceneManager.LoadScene(_currentLevel, LoadSceneMode.Additive);
    }

    [PropertySpace(5)]
    [Button]
    public void LoadLevelByName(string levelName)
    {
        SceneManager.UnloadSceneAsync(_currentLevel);
        _currentLevel = levelName;
        SceneManager.LoadScene(_currentLevel, LoadSceneMode.Additive);
    }

    [PropertySpace(5)]
    [Button]
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}