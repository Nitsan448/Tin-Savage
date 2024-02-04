using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeRunner : MonoBehaviour
{
    private BehaviourTree _tree;
    [SerializeField] private FollowPlayerNode _followPlayerNode;

    private void Start()
    {
        _tree = ScriptableObject.CreateInstance<BehaviourTree>();
        _tree.RootNode = _followPlayerNode;
    }

    private void Update()
    {
        _tree.Update();
    }

    private void FixedUpdate()
    {
        _tree.FixedUpdate();
    }
}