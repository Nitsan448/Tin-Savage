using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : Enemy
{
    protected override void DoOnDeath()
    {
        // GameManager.Instance.State = EGameState.Finished;
    }
}