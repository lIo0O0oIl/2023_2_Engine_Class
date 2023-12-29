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

        CameraManager manager = CameraManager.Instance;

        //임계값을 넘어서 떨어지고 있고
        bool overCameraThreshold = _rigidbody.velocity.y < manager.fallSpeedYDampingChangeThreshold;

        //현재 댐핑중이 아니라면
        if (overCameraThreshold && !manager.IsLerpingYDamping && !manager.LerpedFromPlayerFalling)
        {
            manager.LerpYDamping(true);
        }

        // 땅바닥을 감지해야 끝남.
        if (_player.IsGroundDetected())
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }

        // 벽 감지시 벽 슬라이드 모드

        // 여기다가도 xInput 입력 시 좌우 움직이는 기능 만들기
    }

    public override void Exit()
    {
        CameraManager.Instance.LerpedFromPlayerFalling = false;
        CameraManager.Instance.LerpYDamping(false); //원상복귀
        base.Exit();
    }

}
