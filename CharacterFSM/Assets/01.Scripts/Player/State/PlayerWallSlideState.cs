using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, string animationBoolName) : base(player, stateMachine, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // 벽에 붙었을 때 점프를 할 수 있어야해 플레이어 인풋의 점프 이벤트를 핸들링하는 함수가 있어야함.
        _player.PlayerInput.JumpEvent += HandleJumpEvent;
    }

    private void HandleJumpEvent()
    {
        // 그냥 점프 상태로 변경만 해주면 됨
        _stateMachine.ChangeState(PlayerStateEnum.WallJump);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        float xInput = _player.PlayerInput.XInput;
        float yInput = _player.PlayerInput.YInput;

        if (yInput < 0)
        {
            _player.SetVelocity(0, _rigidbody.velocity.y);
        }
        else
        {
            _player.SetVelocity(0, _rigidbody.velocity.y * 0.6f);
        }

        // 다른 방향으로 방향키를 눌렀다면 (FocingDriection 과 다른 방향으로 눌렀다면)
        // Idle 상태로 전환
        //if (xInput + _player.FacingDirection == 0)
        if (Mathf.Abs(xInput + _player.FacingDirection) < 0.5f)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }

        // 땅에 닿으면  Idle 로 전환
        if (_player.IsGroundDetected())
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    public override void Exit()
    {
        base.Exit();
        _player.PlayerInput.JumpEvent -= HandleJumpEvent;
    }

}
