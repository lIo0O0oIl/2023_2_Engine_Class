using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTVisual
{
    public class BehaviourTreeRunner : MonoBehaviour
    {
        public BehaviourTree tree;
        private EnemyBrain _brain;

        private void Awake()
        {
            _brain = GetComponent<EnemyBrain>();
        }

        private void Start()
        {
            tree = tree.Clone();        // 클론을 떠서 넣음
            tree.Bind(_brain);        // 이 순간에 클론노드 전체에 블랙보드가 복사가 된다.
            // EnemyBrain
        }

        private void Update()
        {
            tree.Update();
        }
    }
}
