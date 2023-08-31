using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadingNode : Node
{
    private Gun _gun;
    private float _reloadTime;
    private float _currentTime;
    private EnemyBrain _brain;

    public ReloadingNode(EnemyBrain brain, Gun gun, float reloadTime)
    {
        _brain = brain;
        _gun = gun;
        _reloadTime = reloadTime;
        _code = NodeActionCode.RELOADING;
    }

    public override NodeState Evaluate()
    {
        if (_brain.currentCode != _code)
        {
            _currentTime = 0;
            _brain.currentCode = _code;
            _brain.TryToTalk("Reloading!! Cover me!!");
        }

        _currentTime += Time.deltaTime;

        if (_currentTime >= _reloadTime)
        {
            _gun.Reload();
            return NodeState.SUCCESS;
        }

        return NodeState.RUNNING;
    }
}
