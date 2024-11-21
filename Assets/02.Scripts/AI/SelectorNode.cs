using UnityEngine;

public class SelectorNode : CompositeNode
{
    public override NodeState Evaluate()
    {
        foreach (var child in children)
        {
            NodeState state = child.Evaluate();
            if (state == NodeState.Success)
                return NodeState.Success;

            if (state == NodeState.Running)
                return NodeState.Running;
        }
        return NodeState.Failure;
    }
}
