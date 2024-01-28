using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.Die();
            return;
        }

        if (other.TryGetComponent(out Enemy enemy))
        {
            enemy.Die(false);
        }
    }
}