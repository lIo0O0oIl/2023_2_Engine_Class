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
        // ���� �پ��� �� ������ �� �� �־���� �÷��̾� ��ǲ�� ���� �̺�Ʈ�� �ڵ鸵�ϴ� �Լ��� �־����.
        _player.PlayerInput.JumpEvent += HandleJumpEvent;
    }

    private void HandleJumpEvent()
    {
        // �׳� ���� ���·� ���游 ���ָ� ��
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

        // �ٸ� �������� ����Ű�� �����ٸ� (FocingDriection �� �ٸ� �������� �����ٸ�)
        // Idle ���·� ��ȯ
        //if (xInput + _player.FacingDirection == 0)
        if (Mathf.Abs(xInput + _player.FacingDirection) < 0.5f)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }

        // ���� ������  Idle �� ��ȯ
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
