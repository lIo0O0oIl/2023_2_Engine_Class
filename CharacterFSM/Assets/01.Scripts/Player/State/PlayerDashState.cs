using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    private float _dashStartTime;
    private float _dashDirection;

    public PlayerDashState(Player player, PlayerStateMachine stateMachine, string animationBoolName) : base(player, stateMachine, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        float xInput = _player.PlayerInput.XInput;
        _dashDirection = Mathf.Abs(xInput) >= 0.05f  ? xInput : _player.FacingDirection;
        _dashStartTime = Time.time;

        SkillManager.Instance.GetSkill<CloneSkill>().CreateCloneOnDashStart();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        _player.SetVelocity(_player.dashSpeed * _dashDirection, 0);

        if (_dashStartTime + _player.dashDuration <= Time.time)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    public override void Exit()
    {
        _player.StopImmediately(false);
        SkillManager.Instance.GetSkill<CloneSkill>().CreateCloneOnDashEnd();
        base.Exit();
    }
}
