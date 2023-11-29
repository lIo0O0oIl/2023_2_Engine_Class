using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerGroundState : PlayerState
{
    protected PlayerGroundState(Player player, PlayerStateMachine stateMachine, string animationBoolName) : base(player, stateMachine, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.PlayerInput.JumpEvent += HandleJumpInput;
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void Exit()
    {
        base.Exit();
        _player.PlayerInput.JumpEvent -= HandleJumpInput;
    }

    #region �Է� ó�� �ڵ鷯
    private void HandleJumpInput()
    {
        if (_player.IsGroundDetected())     // �ٴڿ� ������� ��
        {
            _stateMachine.ChangeState(PlayerStateEnum.Jump);
        }
    }
    #endregion
}
