using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class CompositeNode : Node
{
    public List<Node> Children = new();
}