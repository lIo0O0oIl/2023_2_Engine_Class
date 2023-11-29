using System.Collections.Generic;
using UnityEngine;

public enum PlayerStateEnum
{
    Idle,
    Move,
    Dash,
    Jump,
    Fall
}

public class PlayerStateMachine
{
    // 현재 상태
    public PlayerState CurrentState { get; private set; }
    // 이넘과 해당하는 클래스 인스턴스를 저장하는 딕셔너리
    public Dictionary<PlayerStateEnum, PlayerState> StateDictionary = new Dictionary<PlayerStateEnum, PlayerState>();

    private Player _player;

    public void Initialize(PlayerStateEnum startState, Player player)
    {
        _player = player;
        CurrentState = StateDictionary[startState];
        CurrentState.Enter();       // 시작 상태 넣어주고 시작
    }

    public void ChangeState(PlayerStateEnum newState)
    {
        CurrentState.Exit();
        CurrentState = StateDictionary[newState];
        CurrentState.Enter();
    }

    public void AddState(PlayerStateEnum state, PlayerState playerState)
    {
        StateDictionary.Add(state, playerState);
    }
}
