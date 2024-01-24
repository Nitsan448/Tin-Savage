using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else
        {
            Debug.LogWarning(
                $"Multiple instances of singleton of type {typeof(T)} found in scene. Destroying the current one.");
            Destroy(gameObject);
        }

        DoOnAwake();
    }

    protected virtual void DoOnAwake()
    {
    }
}