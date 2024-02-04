using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitNode : ActionNode
{
    public float Duration = 1;
    public float StartTime = 0;

    protected override void OnStart()
    {
        StartTime = Time.time;
    }

    protected override void OnStop()
    {
    }

    protected override EState OnUpdate()
    {
        return Time.time - StartTime > Duration ? EState.Success : EState.Running;
    }

    protected override void OnFixedUpdate()
    {
    }
}