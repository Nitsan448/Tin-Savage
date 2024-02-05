using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class FinalBoss : Enemy
{
    protected override void DoOnDeath()
    {
        if (SceneReferencer.Instance.Player != null)
        {
            Transitioner.Instance.Win().Forget();
        }
    }
}