using System.Collections.Generic;
using UnityEngine;

public abstract class CompositeNode : Node
{
    protected List<Node> children = new List<Node>();

    public void AddChild(Node node)
    {
        children.Add(node);
    }
}
