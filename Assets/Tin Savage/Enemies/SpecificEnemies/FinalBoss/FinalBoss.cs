using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class FinalBoss : Enemy
{
    protected override void DoOnDeath()
    {
        Transitioner.Instance.Win().Forget();
    }
}