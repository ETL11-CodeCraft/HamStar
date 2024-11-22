using UnityEngine;

public class SequenceNode : CompositeNode
{
    public override NodeState Evaluate()
    {
        foreach (var child in children)
        {
            NodeState state = child.Evaluate();
            if (state == NodeState.Failure)
                return NodeState.Failure;

            if (state == NodeState.Running)
                return NodeState.Running;
        }
        return NodeState.Success;
    }
}
