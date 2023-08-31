using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedReloadingNode : Node
{
    private Gun _gun;

    public NeedReloadingNode(Gun gun)
    {
        _gun = gun;
    }

    public override NodeState Evaluate()
    {
        // 총알이 0이면 리로드
        return _gun.IsEmpty ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
