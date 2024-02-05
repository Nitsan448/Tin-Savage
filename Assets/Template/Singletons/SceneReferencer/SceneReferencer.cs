using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneReferencer : ASingleton<SceneReferencer>
{
    public Player Player;
    public Danger Danger;
    public GameObject KeysParent;
    public WaveManager WaveManager;
}