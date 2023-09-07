using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTVisual
{
    [CreateAssetMenu(menuName ="BehaviourTree/Tree")]
    public class BehaviourTree : ScriptableObject
    {
        public Node rootNode;
        public Node.State treeState = Node.State.RUNNING;

        public Node.State Update()
        {
            if (rootNode.state == Node.State.RUNNING)
            {
                treeState = rootNode.Update();
            }
            return treeState;
        }
    }
}
