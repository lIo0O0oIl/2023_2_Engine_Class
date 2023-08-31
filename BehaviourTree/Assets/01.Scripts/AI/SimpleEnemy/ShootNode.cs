using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootNode : Node
{
    private NavMeshAgent _agent;
    private EnemyBrain _brain;
    private float _coolTime = 0;
    private float _lastFireTime = 0;
    private Gun _gun;

    public ShootNode(NavMeshAgent agent, EnemyBrain brain, float coolTime, Gun gun)
    {
        _agent = agent;
        _brain = brain;
        _coolTime = coolTime;
        _code = NodeActionCode.ShOOT;
        _gun = gun;
    }

    public override NodeState Evaluate()
    {
        _agent.isStopped = true;

        if (_brain.currentCode != _code)
        {
            _brain.TryToTalk("Attack", 1f);
            _brain.currentCode = _code;
        }

        if (_lastFireTime + _coolTime <= Time.time)
        {
            Debug.Log("���ݼ���");
            Vector3 lookDir = _brain.Target.position - _brain.transform.position;
            lookDir.y = 0;
            _brain.transform.rotation = Quaternion.LookRotation(lookDir);

            if (!_gun.IsEmpty)
            {
                _brain.Attack();
                _gun.currentAmmo--;
            }

            _lastFireTime = Time.time;
        }

        return NodeState.RUNNING;
    }
}
