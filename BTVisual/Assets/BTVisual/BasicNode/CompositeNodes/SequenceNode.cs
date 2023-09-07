using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTVisual
{
    public class SequenceNode : CompositeNode
    {
        public int _current = 0;

        protected override void OnStart()
        {
            _current = 0;
        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            /*
            ���� ������ ���ϵ带 ���ͼ� �����ϰ� �� ����� ���� ���� �ؾ� �Ѵ�.
            
            Running => Running
            FAILURE => failure
            SUCCESS => ���� �ڽ����� �Ѿ �غ� �ؾ���

            ���� �ڽĳѹ��� ��ü �ڽĳѹ�
            Success
            �װ� �ƴϸ� �ڽ��� �����Ŵϱ�
            Running
            */

            var child = children[_current];
            switch (child.Update())
            {
                case State.RUNNING:
                    return State.RUNNING;
                case State.FAILURE:
                    return State.FAILURE;
                case State.SUCCESS:
                    _current++;
                    break;
            }

            return _current == children.Count ? State.SUCCESS : State.RUNNING;
        }
    }
}
