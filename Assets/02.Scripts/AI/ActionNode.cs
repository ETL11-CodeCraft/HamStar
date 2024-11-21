using System;
using UnityEngine;

public class ActionNode : Node
{
    private Func<NodeState> action;

    public ActionNode(Func<NodeState> action)
    {
        this.action = action;
    }

    public override NodeState Evaluate()
    {
        return action.Invoke();
    }
}
