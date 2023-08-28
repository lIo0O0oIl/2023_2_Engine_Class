using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class Selector : Node
    {
        protected List<Node> _nodes = new List<Node>();

        public Selector(List<Node> nodes)
        {
            _nodes = nodes;
        }

        public override NodeState Evaluate()
        {
            foreach (var n in _nodes)
            {
                switch (n.Evaluate())
                {
                    case NodeState.RUNNING:
                        _nodeState = NodeState.RUNNING;     // �ϴ� ���̴ϱ� �������� ���� ��
                        return _nodeState;
                    case NodeState.SUCCESS:
                        _nodeState = NodeState.SUCCESS;     // �����ϸ� �������� ����
                        return _nodeState;
                    case NodeState.FAILURE:     // �����ϸ� ���� ��带 ��������?
                        break;
                }
            }
            // ������� �Դٴ°� �� ����
            _nodeState = NodeState.FAILURE;
            return _nodeState;
        }
    }
}


