using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneReferencer : ASingleton<SceneReferencer>
{
    public Player Player;

    protected override void DoOnAwake()
    {
        base.DoOnAwake();
        if (Player == null)
        {
            Player = FindObjectOfType<Player>();
        }
    }
}