using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : ScriptableObject
{
    public enum EState
    {
        Running,
        Failure,
        Success
    }

    public EState State = EState.Running;
    public bool Started = false;

    public EState Update()
    {
        if (!Started)
        {
            OnStart();
            Started = true;
        }

        State = OnUpdate();

        if (State == EState.Success || State == EState.Failure)
        {
            OnStop();
        }

        return State;
    }

    public void FixedUpdate()
    {
        if (!Started)
        {
            return;
        }

        OnFixedUpdate();
    }

    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract EState OnUpdate();
    protected abstract void OnFixedUpdate();
}