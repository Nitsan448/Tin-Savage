using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer2D
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public abstract class APlayerTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                DoOnTriggerEnter(player);
            }
        }

        protected virtual void DoOnTriggerEnter(Player player)
        {
        }
    }
}