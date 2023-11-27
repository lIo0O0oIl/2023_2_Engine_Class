using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 상태를 기반으로 플레이어를 만들꺼니까 PlayerState 라는 클래스를 하나 만들고 그걸 상속받아서 Move 와 Idle을 만들것
// 이걸 리플렉션으로 한꺼번에 불러올 것임.
public class Player : MonoBehaviour
{
    [Header("Setting values")]
    public float moveSpeec = 12f;
    public float jumpForce = 12f;
    public float dashDuration = 0.4f;
    public float dashSpeed = 20f;

    public PlayerStateMachine StateMachine { get; private set; }

    #region 컴포넌트 영역
    public Animator AnimatorCompo { get; private set; }
    public Rigidbody2D RigidbodyCompo { get; private set; }
    public CapsuleCollider2D ColliderCompo { get; private set; }
    #endregion

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
    }
}
