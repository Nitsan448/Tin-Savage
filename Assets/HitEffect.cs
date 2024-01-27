using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class HitEffect : ASingleton<HitEffect>
{
    public async UniTask OnHit(Material material)
    {
        material.SetFloat("_emissioin", 1);
        // await UniTask.Yield();
        await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        material.SetFloat("_emissioin", 0);
    }
}