using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int _comboCounter;      // 현재 콤보 수치
    private float _lastAttackTime;      // 마지막으로 공격했던 시간
    private float _comboWindow = 0.8f;      // 공격이 들어가고 이 시간이 지나면 콤보가 끊어짐, 콤보 윈도우, 코요테 타이머또 있음 = 바닥에서 떨어졌을 때 이 시간이 지날 때 까지는 점프가 가능하게 됨. 사용자 경험 좋게 하기 위해서
    private readonly int _comboCounterHash = Animator.StringToHash("ComboCounter");

    public PlayerPrimaryAttackState(Player player, PlayerStateMachine stateMachine, string animationBoolName) : base(player, stateMachine, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (_comboCounter > 2 || Time.time >= _lastAttackTime + _comboWindow)
        {
            _comboCounter = 0;      // 콤보 초기화 조건에 따라 콤보 초기화
        }
        _player.AnimatorCompo.SetInteger(_comboCounterHash, _comboCounter);
        _player.AnimatorCompo.speed = _player.attackSpeed;

        // 공격방향을 알아내라. 아무키도 안눌렸다면 facingDirection
        // 만약 키가 눌리고 있다면 해당방향으로 공격을 정하고
        float attackDirection = _player.FacingDirection;
        float xInput = _player.PlayerInput.XInput;
        if (Mathf.Abs(xInput) > 0.05f)
        {
            attackDirection = Mathf.Sign(xInput);
        }

        // 플레이어를 약간 전진하게 만들어주자.
        // 전진이 끝나면 (0.1초가량이 흐르면) StopImmediately 호출)
        Vector2 move = _player.attackMovement[_comboCounter];
        _player.SetVelocity(move.x * attackDirection, move.y);

        _player.StartDelayCall(0.1f, () => _player.StopImmediately(false));
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_triggerCalled)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    public override void Exit()
    {
        ++_comboCounter;
        _lastAttackTime = Time.time;
        _player.AnimatorCompo.speed = 1;
        base.Exit();
    }

}
