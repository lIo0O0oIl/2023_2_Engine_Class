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
                        _nodeState = NodeState.RUNNING;     // 하는 중이니까 다음으로 가지 마
                        return _nodeState;
                    case NodeState.SUCCESS:
                        _nodeState = NodeState.SUCCESS;     // 성공하면 다음으로 가자
                        return _nodeState;
                    case NodeState.FAILURE:     // 실패하면 다음 노드를 봐야하지?
                        break;
                }
            }
            // 여기까지 왔다는건 다 실패
            _nodeState = NodeState.FAILURE;
            return _nodeState;
        }
    }
}


