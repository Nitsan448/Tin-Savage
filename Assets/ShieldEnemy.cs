using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy : Enemy
{
    [SerializeField] private GameObject _shield;

    protected override void DoOnHit()
    {
        base.DoOnHit();
        _shield.SetActive(false);
    }
}