using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfiguration : ASingleton<GameConfiguration>
{
    public AnimationCurve PushCurve;
    public bool InfiniteDashes = false;
    public int StartingWave = 0;
    public float TimeBetweenWavesScale = 1;
}