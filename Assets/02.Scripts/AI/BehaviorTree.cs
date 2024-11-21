using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree
{
    private Node root;
    private Node _current;
    private Stack<Node> _stack = new Stack<Node>();

    public BehaviorTree SetRoot(Node node)
    {
        root = node;
        _current = node;
        _stack.Push(node);

        return this;
    }

    public void Evaluate()
    {
        root.Evaluate();
    }

    public BehaviorTree Selector()
    {
        if(_current is CompositeNode)
        {
            var selector = new SelectorNode();
            ((CompositeNode)_current).AddChild(selector);
            _stack.Push(selector);
            _current = selector;
        }
        else
        {
            Debug.LogError($"Selector Node를 붙일 수 없습니다.");
        }

        return this;
    }

    public BehaviorTree Sequence()
    {
        if (_current is CompositeNode)
        {
            var Sequence = new SequenceNode();
            ((CompositeNode)_current).AddChild(Sequence);
            _stack.Push(Sequence);
            _current = Sequence;
        }
        else
        {
            Debug.LogError($"Sequence Node를 붙일 수 없습니다.");
        }

        return this;
    }

    public BehaviorTree CloseComposite()
    {
        if (_stack.Count > 0)
        {
            _stack.Pop();
        }
        if (_stack.Count > 0)
        {
            _current = _stack.Peek();
        }

        return this;
    }

    public BehaviorTree Node(Func<NodeState> action)
    {
        if(_current is CompositeNode)
        {
            var node = new ActionNode(action);
            ((CompositeNode)_current).AddChild(node);
        }

        return this;
    }
}
