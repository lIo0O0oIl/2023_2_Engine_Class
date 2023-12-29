using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// 상태를 기반으로 플레이어를 만들꺼니까 PlayerState 라는 클래스를 하나 만들고 그걸 상속받아서 Move 와 Idle을 만들것
// 이걸 리플렉션으로 한꺼번에 불러올 것임.
public class Player : MonoBehaviour
{
    public Action<int> OnFlip;

    [Header("Setting values")]
    public float moveSpeed = 12f;
    public float jumpForce = 12f;
    public float dashDuration = 0.4f;
    public float dashSpeed = 20f;

    [Header("Collistion info")]
    [SerializeField] protected Transform _groundChecker;
    [SerializeField] protected float _groundCheckDistance;
    [SerializeField] protected LayerMask _wathIsGround;
    [SerializeField] protected Transform _wallChecker;
    [SerializeField] protected float _wallCheckDistance;

    public PlayerStateMachine StateMachine { get; private set; }

    [SerializeField] private InputReader InputReader;
    public InputReader PlayerInput => InputReader;

    #region 컴포넌트 영역
    public Animator AnimatorCompo { get; private set; }
    public Rigidbody2D RigidbodyCompo { get; private set; }
    public CapsuleCollider2D ColliderCompo { get; private set; }

    [field: SerializeField] public PlayerStat Stat { get; private set; }
    #endregion

    [Header("attack info")]
    public float attackSpeed;
    public Vector2[] attackMovement;

    public int FacingDirection { get; private set; } = 1;       // 오른쪽이 1, 왼쪽이 -1

    protected bool _facingRight = true;

    protected virtual void Awake()
    {
        Transform visualTrm = transform.Find("Visual");
        AnimatorCompo = visualTrm.GetComponent<Animator>();
        RigidbodyCompo = GetComponent<Rigidbody2D>();
        ColliderCompo = GetComponent<CapsuleCollider2D>();

        StateMachine = new PlayerStateMachine();

        foreach (PlayerStateEnum state in Enum.GetValues(typeof(PlayerStateEnum)))      // 런타임에서 뽑아오거나 만들어주거나. 기본거보다 겁나 느림. 찾아온 값을 캐싱해서 사용해주는 것이 좋음.
        {
            string typeName = state.ToString();
            Type t = Type.GetType($"Player{typeName}State");        // PlayerIdleState 같은 스크립트 가져와줌

            PlayerState newState = Activator.CreateInstance(t, this, StateMachine, typeName) as PlayerState;

            StateMachine.AddState(state, newState);
        }

        Stat = Instantiate(Stat);       // 복제
        Stat.SetOwner(this);        // 소유권 집어넣기
    }

    private void OnEnable()
    {
        PlayerInput.DashEvent += HandleDashInput;
        PlayerInput.CrystalSkillEvent += HandleCrystalSkillInput;
    }

    private void OnDisable()
    {
        PlayerInput.DashEvent -= HandleDashInput;
        PlayerInput.CrystalSkillEvent -= HandleCrystalSkillInput;
    }

    #region 딜레이 코루틴 코드
    public Coroutine StartDelayCall(float delayTime, Action Callback)
    {
        return StartCoroutine(DelayCallCoroutine(delayTime, Callback));
    }

    private IEnumerator DelayCallCoroutine(float delayTime, Action Callback)
    {
        yield return new WaitForSeconds(delayTime);
        Callback?.Invoke();
    }
    #endregion

    #region 키입력 핸들러들
    private void HandleDashInput()
    {
        DashSkill skill = SkillManager.Instance.GetSkill<DashSkill>();

        if (skill != null &&  skill.AttemptUseSkill()){
            // 스킬 시스템 구현시에 쿨타임 체크해서 해당 스킬 사용가능할 때 사용하도록
            StateMachine.ChangeState(PlayerStateEnum.Dash);
        }
    }

    private void HandleCrystalSkillInput()
    {
        SkillManager.Instance.GetSkill<CrystalSkill>()?.AttemptUseSkill();
    }
    #endregion

    private void Start()
    {
        StateMachine.Initialize(PlayerStateEnum.Idle, this);
    }

    protected void Update()
    {
        StateMachine.CurrentState.UpdateState();

        // 디버그 코드
/*        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            Stat.IncreaseStatBy(10, 5f, Stat.GetStatByType(StatType.strength));
        }*/
    }

    #region 속도 제어
    public void SetVelocity(float x, float y, bool doNotFlip = false)
    {
        // 넉백상태일 경우는 안해줌
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

    #region 플립 제어
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
        FacingDirection = FacingDirection * -1;        // 반전
        _facingRight = !_facingRight;
        transform.Rotate(0, 180, 0);
        OnFlip?.Invoke(FacingDirection);
    }
    #endregion

    #region 충돌 체크 부분
    public virtual bool IsGroundDetected() =>
        Physics2D.Raycast(_groundChecker.position, Vector2.down, _groundCheckDistance, _wathIsGround);

    public virtual bool IsWallDetected() =>
       Physics2D.Raycast(_wallChecker.position, Vector2.right * FacingDirection, _wallCheckDistance, _wathIsGround);
    #endregion

    public void AnimationFinishTrigger()
    {
        StateMachine.CurrentState.AnimationFinishTrigger();
    }

#if UNITY_EDITOR
    protected virtual void OnDrawGizmos()
    {
        if (_groundChecker != null)
            Gizmos.DrawLine(_groundChecker.position,
                _groundChecker.position + new Vector3(0, -_groundCheckDistance, 0));
        if (_wallChecker != null)
            Gizmos.DrawLine(_wallChecker.position,
                _wallChecker.position + new Vector3(_wallCheckDistance, 0, 0));
    }
#endif
}
