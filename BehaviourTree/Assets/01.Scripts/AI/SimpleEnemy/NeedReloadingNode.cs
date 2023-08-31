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
        // �Ѿ��� 0�̸� ���ε�
        return _gun.IsEmpty ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
