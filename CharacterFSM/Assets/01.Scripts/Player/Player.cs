using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���¸� ������� �÷��̾ ���鲨�ϱ� PlayerState ��� Ŭ������ �ϳ� ����� �װ� ��ӹ޾Ƽ� Move �� Idle�� �����
// �̰� ���÷������� �Ѳ����� �ҷ��� ����.
public class Player : MonoBehaviour
{
    [Header("Setting values")]
    public float moveSpeed = 12f;
    public float jumpForce = 12f;
    public float dashDuration = 0.4f;
    public float dashSpeed = 20f;

    public PlayerStateMachine StateMachine { get; private set; }

    [SerializeField] private InputReader InputReader;
    public InputReader PlayerInput => InputReader;

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

    private void OnEnable()
    {
        PlayerInput.DashEvent += HandleDashInput;
    }

    private void OnDisable()
    {
        PlayerInput.DashEvent -= HandleDashInput;
    }

    #region Ű�Է� �ڵ鷯��
    private void HandleDashInput()
    {
        // ��ų �ý��� �����ÿ� ��Ÿ�� üũ�ؼ� �ش� ��ų ��밡���� �� ����ϵ���
        StateMachine.ChangeState(PlayerStateEnum.Dash);
    }
    #endregion

    private void Start()
    {
        StateMachine.Initialize(PlayerStateEnum.Idle, this);
    }

    protected void Update()
    {
        StateMachine.CurrentState.UpdateState();
    }

    #region �ӵ� ����
    public void SetVelocity(float x, float y, bool doNotFlip = false)
    {
        // �˹������ ���� ������
        RigidbodyCompo.velocity = new Vector2(x, y);
        if (!doNotFlip)
        {
            FlipController(x);
        }
    }

    public void StopImmediately(bool withYAxis)
    {
        if (withYAxis)
        {
            RigidbodyCompo.velocity = Vector2.zero;
        }
        else
        {
            RigidbodyCompo.velocity = new Vector2(0, RigidbodyCompo.velocity.y);
        }
    }
    #endregion

    #region �ø� ����
    public void FlipController(float x)
    {
        bool goToRight = x > 0 && !_facingRight;
        bool goToLeft = x < 0 && _facingRight;
        if (goToRight || goToLeft)
        {
            Flip();
        }
    }

    public void Flip()
    {
        FacingDirection = FacingDirection * -1;        // ����
        _facingRight = !_facingRight;
        transform.Rotate(0, 180, 0);
    }
    #endregion
}
