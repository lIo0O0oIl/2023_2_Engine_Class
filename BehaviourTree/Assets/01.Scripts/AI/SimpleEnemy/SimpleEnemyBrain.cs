using BehaviourTree;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyBrain : EnemyBrain
{
    private Node _topNode;
    private NodeState _beforeTopState;

    private EnemyAttack _enemyAttack;
    private Gun _gun;

    protected override void Awake()
    {
        base.Awake();
        _enemyAttack = GetComponent<EnemyAttack>();
        _gun = transform.Find("Gun").GetComponent<Gun>();
    }

    public override void Attack()
    {
        _enemyAttack.Attack();
    }

    protected  void Start()
    {
        ConstructAITree();      // �ٽ���
    }

    private void ConstructAITree()
    {
        Transform me = transform;       // Ʈ���������� ĳ����.

        NeedReloadingNode needReloadingNode = new NeedReloadingNode(_gun);
        ReloadingNode reloadingNode = new ReloadingNode(this, _gun, 3f);
        Sequence reloadSeq = new Sequence(nodes:new List<Node> { needReloadingNode, reloadingNode});

        RangeNode shootingRange = new RangeNode(7f, _targetTrm, body: me);
        ShootNode shootNode = new ShootNode(NavAgent, brain: this, coolTime: 1.5f, _gun); // ���� �� 1.5��
        Sequence shootSeq = new Sequence(nodes: new List<Node> { shootingRange, shootNode });

        RangeNode chasingRange = new RangeNode(10f, _targetTrm, body: me);
        ChaseNode chaseNode = new ChaseNode(_targetTrm, NavAgent, brain: this);
        Sequence chaseSeq = new Sequence(nodes: new List<Node> { chasingRange, chaseNode });
        
        _topNode = new Selector(nodes:new List<Node> { reloadSeq, shootSeq, chaseSeq });
    }

    private void Update()
    {
        _topNode.Evaluate();
        if (_topNode.NodeState == NodeState.FAILURE && _beforeTopState != NodeState.FAILURE)
        {
            TryToTalk(text:"�ƹ��͵� �Ұ� ����");
            NavAgent.isStopped = true;
            currentCode = NodeActionCode.NONE;
        }

        _beforeTopState = _topNode.NodeState;
    }
}
