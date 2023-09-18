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
            tree = tree.Clone();        // Ŭ���� ���� ����
            tree.Bind(_brain);        // �� ������ Ŭ�г�� ��ü�� �����尡 ���簡 �ȴ�.
            // EnemyBrain
        }

        private void Update()
        {
            tree.Update();
        }
    }
}
