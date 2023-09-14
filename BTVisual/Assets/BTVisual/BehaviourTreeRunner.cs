using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTVisual
{
    public class BehaviourTreeRunner : MonoBehaviour
    {
        public BehaviourTree tree;

        private void Start()
        {
            tree = tree.Clone();        // 클론을 떠서 넣음



            //_tree = ScriptableObject.CreateInstance<BehaviourTree>();

            //var dNode1 = ScriptableObject.CreateInstance<DebugNode>();
            //dNode1.message = "GGM Message!";

            //var rNode = ScriptableObject.CreateInstance<RepeatNode>();
            //rNode.child = dNode1;

            //_tree.rootNode = rNode;


            /*var wait1 = ScriptableObject.CreateInstance<WaitNode>();
            wait1.duration = 1;
            var debug1 = ScriptableObject.CreateInstance<DebugNode>();
            debug1.message = "Hello GGM1";

            var wait2 = ScriptableObject.CreateInstance<WaitNode>();
            wait2.duration = 2;
            var debug2 = ScriptableObject.CreateInstance<DebugNode>();
            debug2.message = "Hello GGM2";

            var seq = ScriptableObject.CreateInstance<SequenceNode>();
            seq.children.Add(wait1);
            seq.children.Add(debug1);
            seq.children.Add(wait2);
            seq.children.Add(debug2);

            var repeatNode = ScriptableObject.CreateInstance<RepeatNode>();
            repeatNode.child = seq;

            _tree.rootNode = repeatNode;*/

        }

        private void Update()
        {
            tree.Update();
        }
    }
}
