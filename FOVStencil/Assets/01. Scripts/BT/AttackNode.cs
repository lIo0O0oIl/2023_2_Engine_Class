using BTVisual;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackNode : ActionNode
{
    public float coolTime;

    private float _nextFireTime;

    protected override void OnStart()
    {
        context.agent.isStopped = true;
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        if (context.agent.isStopped == false)
        {
            context.agent.isStopped = true;
        }

        if (Time.time > _nextFireTime)
        {
            brain.Attack();
            _nextFireTime = Time.time + coolTime + Random.Range(-0.2f, 0.2f);
            return State.SUCCESS;
        }
        else
        {
            return State.RUNNING;
        }
    }
}
