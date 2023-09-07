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
            지금 실행할 차일드를 골라와서 실행하고 그 결과에 따라서 같이 해야 한다.
            
            Running => Running
            FAILURE => failure
            SUCCESS => 다음 자식으로 넘어갈 준비를 해야함

            현재 자식넘버가 전체 자식넘버
            Success
            그게 아니면 자식이 남은거니까
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
