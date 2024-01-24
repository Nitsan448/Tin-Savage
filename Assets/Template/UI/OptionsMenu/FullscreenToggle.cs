using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullscreenToggle : MonoBehaviour
{
    void Start()
    {
        GetComponent<Toggle>().onValueChanged.AddListener(delegate { Screen.fullScreen = !Screen.fullScreen; });
    }
}
