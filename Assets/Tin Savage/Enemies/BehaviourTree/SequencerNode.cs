using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencerNode : CompositeNode
{
    private int _currentChildIndex;

    protected override void OnStart()
    {
        _currentChildIndex = 0;
    }

    protected override void OnStop()
    {
    }

    protected override EState OnUpdate()
    {
        Node child = Children[_currentChildIndex];
        switch (child.Update())
        {
            case EState.Running:
                return EState.Running;
            case EState.Failure:
                return EState.Failure;
            case EState.Success:
                _currentChildIndex++;
                break;
        }

        return _currentChildIndex == Children.Count ? EState.Success : EState.Running;
    }

    protected override void OnFixedUpdate()
    {
    }
}