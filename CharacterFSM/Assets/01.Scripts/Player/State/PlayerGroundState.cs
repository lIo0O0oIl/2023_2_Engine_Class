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
        _player.PlayerInput.AttackEvent += HandleAttackInput;
    }


    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void Exit()
    {
        base.Exit();
        _player.PlayerInput.JumpEvent -= HandleJumpInput;
        _player.PlayerInput.AttackEvent -= HandleAttackInput;
    }

    #region 입력 처리 핸들러
    private void HandleJumpInput()
    {
        if (_player.IsGroundDetected())     // 바닥에 닿아있을 때
        {
            _stateMachine.ChangeState(PlayerStateEnum.Jump);
        }
    }

    private void HandleAttackInput()
    {
        if (_player.IsGroundDetected())
        {
            _stateMachine.ChangeState(PlayerStateEnum.PrimaryAttack);
        }
    }
    #endregion
}
