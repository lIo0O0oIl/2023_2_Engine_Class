using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player player, PlayerStateMachine stateMachine, string animationBoolName) : base(player, stateMachine, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        float xInput = _player.PlayerInput.XInput;

        if (Mathf.Abs(xInput) > 0.05f)
        {
            _player.SetVelocity(_player.moveSpeed * 0.8f * xInput, _player.jumpForce);      // �̰� ������?ū�ϳ���
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
