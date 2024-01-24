using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Logger
{
    [SerializeField] private bool _showLogs;

    public Logger(bool isShowLogs)
    {
        _showLogs = isShowLogs;
    }

    public void Log(object message, Object sender)
    {
        if (_showLogs)
        {
            Debug.Log(message, sender);
        }
    }
}
