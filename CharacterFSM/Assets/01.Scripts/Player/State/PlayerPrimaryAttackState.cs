using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int _comboCounter;      // ���� �޺� ��ġ
    private float _lastAttackTime;      // ���������� �����ߴ� �ð�
    private float _comboWindow = 0.8f;      // ������ ���� �� �ð��� ������ �޺��� ������, �޺� ������, �ڿ��� Ÿ�̸Ӷ� ���� = �ٴڿ��� �������� �� �� �ð��� ���� �� ������ ������ �����ϰ� ��. ����� ���� ���� �ϱ� ���ؼ�
    private readonly int _comboCounterHash = Animator.StringToHash("ComboCounter");

    public PlayerPrimaryAttackState(Player player, PlayerStateMachine stateMachine, string animationBoolName) : base(player, stateMachine, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (_comboCounter > 2 || Time.time >= _lastAttackTime + _comboWindow)
        {
            _comboCounter = 0;      // �޺� �ʱ�ȭ ���ǿ� ���� �޺� �ʱ�ȭ
        }
        _player.AnimatorCompo.SetInteger(_comboCounterHash, _comboCounter);
        _player.AnimatorCompo.speed = _player.attackSpeed;

        // ���ݹ����� �˾Ƴ���. �ƹ�Ű�� �ȴ��ȴٸ� facingDirection
        // ���� Ű�� ������ �ִٸ� �ش�������� ������ ���ϰ�
        float attackDirection = _player.FacingDirection;
        float xInput = _player.PlayerInput.XInput;
        if (Mathf.Abs(xInput) > 0.05f)
        {
            attackDirection = Mathf.Sign(xInput);
        }

        // �÷��̾ �ణ �����ϰ� ���������.
        // ������ ������ (0.1�ʰ����� �帣��) StopImmediately ȣ��)
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
