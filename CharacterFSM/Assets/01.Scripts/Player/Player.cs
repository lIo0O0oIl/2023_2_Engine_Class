using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���¸� ������� �÷��̾ ���鲨�ϱ� PlayerState ��� Ŭ������ �ϳ� ����� �װ� ��ӹ޾Ƽ� Move �� Idle�� �����
// �̰� ���÷������� �Ѳ����� �ҷ��� ����.
public class Player : MonoBehaviour
{
    [Header("Setting values")]
    public float moveSpeec = 12f;
    public float jumpForce = 12f;
    public float dashDuration = 0.4f;
    public float dashSpeed = 20f;

    public PlayerStateMachine StateMachine { get; private set; }

    #region ������Ʈ ����
    public Animator AnimatorCompo { get; private set; }
    public Rigidbody2D RigidbodyCompo { get; private set; }
    public CapsuleCollider2D ColliderCompo { get; private set; }
    #endregion

    public int FacingDirection { get; private set; } = 1;       // �������� 1, ������ -1
    protected bool _facingRight = true;

    protected virtual void Awake()
    {
        Transform visualTrm = transform.Find("Visual");
        AnimatorCompo = visualTrm.GetComponent<Animator>();
        RigidbodyCompo = GetComponent<Rigidbody2D>();
        ColliderCompo = GetComponent<CapsuleCollider2D>();

        StateMachine = new PlayerStateMachine();

        foreach (PlayerStateEnum state in Enum.GetValues(typeof(PlayerStateEnum)))      // ��Ÿ�ӿ��� �̾ƿ��ų� ������ְų�. �⺻�ź��� �̳� ����. ã�ƿ� ���� ĳ���ؼ� ������ִ� ���� ����.
        {
            string typeName = state.ToString();
            Type t = Type.GetType($"Player{typeName}State");        // PlayerIdleState ���� ��ũ��Ʈ ��������

            PlayerState newState = Activator.CreateInstance(t, this, StateMachine, typeName) as PlayerState;

            StateMachine.AddState(state, newState);
        }
    }
}
