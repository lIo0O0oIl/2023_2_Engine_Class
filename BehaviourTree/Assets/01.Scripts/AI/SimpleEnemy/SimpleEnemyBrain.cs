using BehaviourTree;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyBrain : EnemyBrain
{
    private Node _topNode;
    private NodeState _beforeTopState;

    protected  void Start()
    {
        ConstructAITree();      // 핵심임
    }

    private void ConstructAITree()
    {
        Transform me = transform;       // 트랜스폼으로 캐싱함.
        RangeNode shootingRange = new RangeNode(3f, _targetTrm, body: me);
        ShootNode shootNode = new ShootNode(NavAgent, brain: this, coolTime: 1.5f); // 공격 쿨 1.5초
        Sequence shootSeq = new Sequence(nodes: new List<Node> { shootingRange, shootNode });

        RangeNode chasingRange = new RangeNode(10f, _targetTrm, body: me);
        ChaseNode chaseNode = new ChaseNode(_targetTrm, NavAgent, brain: this);
        Sequence chaseSeq = new Sequence(nodes: new List<Node> { chasingRange, chaseNode });

        _topNode = new Selector(nodes:new List<Node> { shootSeq, chaseSeq });
    }

    private void Update()
    {
        _topNode.Evaluate();
        if (_topNode.NodeState == NodeState.FAILURE && _beforeTopState != NodeState.FAILURE)
        {
            TryToTalk(text:"아무것도 할게 없어");
        }

        _beforeTopState = _topNode.NodeState;
    }
}
