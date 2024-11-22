using UnityEngine;

public enum NodeState
{
    Success,
    Running,
    Failure,
}


public abstract class Node
{
    public abstract NodeState Evaluate();
}
