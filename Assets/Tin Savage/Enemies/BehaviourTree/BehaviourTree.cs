using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BehaviourTree : ScriptableObject
{
    public Node RootNode;
    private List<Node> _runningNodes = new();
    public Node.EState TreeState = Node.EState.Running;

    public Node.EState Update()
    {
        if (RootNode.State == Node.EState.Running)
        {
            TreeState = RootNode.Update();
        }

        return TreeState;
    }

    public void FixedUpdate()
    {
        if (RootNode.State == Node.EState.Running)
        {
            RootNode.FixedUpdate();
        }
    }
}