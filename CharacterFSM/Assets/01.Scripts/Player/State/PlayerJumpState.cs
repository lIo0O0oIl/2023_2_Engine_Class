using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, string animationBoolName) : base(player, stateMachine, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.SetVelocity(_rigidbody.velocity.x, _player.jumpForce, true);
    }

    public override void UpdateState()
    {
        base.UpdateState();

        // �����߿� �¿�� �����̱� ���Ѵٸ� ���⿡ �ڵ��ϱ�


        if (_rigidbody.velocity.y < 0)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Fall);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}