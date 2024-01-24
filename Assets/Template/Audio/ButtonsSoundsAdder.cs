using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TODO: refactor or move to editor script/audio manager
public class ButtonsSoundsAdder : MonoBehaviour
{
    [SerializeField] private string _buttonClickSoundName;

    private void Start()
    {
        if (_buttonClickSoundName != "")
        {
            AddSoundToButtons();
        }
    }

    private void AddSoundToButtons()
    {
        foreach (Button button in FindObjectsOfType<Button>(true))
        {
            button.onClick.AddListener(() => AudioManager.Instance.Play(_buttonClickSoundName));
        }
    }
}