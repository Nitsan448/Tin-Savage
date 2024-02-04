using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLogNode : ActionNode
{
    public string Message;

    protected override void OnStart()
    {
        Debug.Log($"ÖnStart{Message}");
    }

    protected override void OnStop()
    {
        Debug.Log($"ÖnStop{Message}");
    }

    protected override EState OnUpdate()
    {
        Debug.Log($"ÖnUpdate{Message}");
        return EState.Success;
    }

    protected override void OnFixedUpdate()
    {
        throw new System.NotImplementedException();
    }
}