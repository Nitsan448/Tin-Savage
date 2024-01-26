using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    [HideInInspector] public int NumberOfEnemiesLeft;

    private void Start()
    {
        Enemy[] enemiesInWave = GetComponentsInChildren<Enemy>();
        foreach (Enemy enemy in enemiesInWave)
        {
            enemy.Init(this);
        }

        NumberOfEnemiesLeft = enemiesInWave.Length;
    }

    public void OnEnemyDeath()
    {
        NumberOfEnemiesLeft--;
        if (NumberOfEnemiesLeft == 0)
        {
            Destroy(gameObject);
        }
    }
}