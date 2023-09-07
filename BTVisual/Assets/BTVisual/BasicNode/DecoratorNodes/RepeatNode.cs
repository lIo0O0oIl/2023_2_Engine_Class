using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTVisual
{
    public class RepeatNode : DecoratorNode
    {
        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            // 자식을 구동시키고 자식의 결과와 상관없이 트리를 구동시키도록
            child.Update();
            return State.RUNNING;
        }
    }
}
