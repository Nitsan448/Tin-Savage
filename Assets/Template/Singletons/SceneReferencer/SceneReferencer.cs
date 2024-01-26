using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneReferencer : ASingleton<SceneReferencer>
{
    public Player Player;
    public Danger danger;

    protected override void DoOnAwake()
    {
        base.DoOnAwake();
        if (Player == null)
        {
            Player = FindObjectOfType<Player>();
        }

        if (danger == null)
        {
            danger = FindObjectOfType<Danger>();
        }
    }
}