using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneReferencer : ASingleton<SceneReferencer>
{
    public Player Player;
    public Danger Danger;

    protected override void DoOnAwake()
    {
        base.DoOnAwake();
        if (Player == null)
        {
            Player = FindObjectOfType<Player>();
        }
    }
}