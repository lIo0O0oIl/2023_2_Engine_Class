using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine _stateMachine;     // 플레이어 상태 머신
    protected Player _player;
    protected Rigidbody2D _rigidbody;       // 편의성을 위해서 참조

    protected int _animBoolHash;
    protected readonly int _yVelocityHash = Animator.StringToHash("y_velocity");

    protected bool _triggerCalled;

    public PlayerState(Player player, PlayerStateMachine stateMachine, string animationBoolName)
    {
        _player = player;
        _stateMachine = stateMachine;
        _animBoolHash = Animator.StringToHash(animationBoolName);
        _rigidbody = _player.RigidbodyCompo;      // 이건 나중에
    }

    public virtual void Enter()
    {
        _triggerCalled = false;
        _player.AnimatorCompo.SetBool(_animBoolHash, true);
    }

    public virtual void UpdateState()
    {
        _player.AnimatorCompo.SetFloat(_yVelocityHash, _rigidbody.velocity.y);
    }

    public virtual void Exit() 
    {
        _player.AnimatorCompo.SetBool(_animBoolHash, false);
    }

    public void AnimationFinishTrigger()
    {
        _triggerCalled = true;
    }
}
