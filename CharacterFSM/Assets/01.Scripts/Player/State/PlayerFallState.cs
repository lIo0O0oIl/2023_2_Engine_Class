using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerAirState
{
    public PlayerFallState(Player player, PlayerStateMachine stateMachine, string animationBoolName) : base(player, stateMachine, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        // 땅바닥을 감지해야 끝이남.
        if (_player.IsGroundDetected())
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }

        // 벽을 감지하면 벽슬라이드모드로 들어가게됨.

        // 여기다가도 xInput 입력 시 죄우 움직임 기능 추가해야해.
    }

    public override void Exit()
    {
        base.Exit();
    }

}
