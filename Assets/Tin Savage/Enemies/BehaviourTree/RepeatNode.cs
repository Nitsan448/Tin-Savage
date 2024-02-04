using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatNode : DecoratorNode
{
    //TODO: add to update
    public int Iterations;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override EState OnUpdate()
    {
        Child.Update();
        return EState.Running;
    }

    protected override void OnFixedUpdate()
    {
    }
}