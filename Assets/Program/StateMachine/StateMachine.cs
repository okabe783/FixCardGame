using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] private InGameView _inGameView;
    private State _currentState; // 現在のState
    private readonly Dictionary<string, State> _states = new(); // 全Phaseを管理する

    private static StateMachine _stateMachine;

    private MulliganPhase _mulliganPhase;
    private TurnStartPhase _turnStartPhase;
    private PlayPhase _playPhase;
    private BattlePhase _battlePhase;
    private TurnEndPhase _turnEndPhase;

    public static StateMachine GetInstance()
    {
        if (_stateMachine == null)
        {
            _stateMachine = FindAnyObjectByType<StateMachine>();
        }

        return _stateMachine;
    }

    private void Awake()
    {
        //ToDo:全てにViewを登録
        AddState("mulligan", new MulliganPhase(_inGameView));
        AddState("turnStart", new TurnStartPhase(_inGameView));
        AddState("play", new PlayPhase());
        AddState("battle", new BattlePhase());
        AddState("turnEnd", new TurnEndPhase());
    }

    private async UniTask Start()
    {
        await ChangeState("mulligan");
    }

    // 状態を登録
    private void AddState(string key, State state)
    {
        _states.TryAdd(key, state);
    }

    //Stateの登録
    public async UniTask ChangeState(string key)
    {
        if (_currentState != null)
        {
            _currentState.Exit(); // 現在の状態を終了
        }

        if (_states.ContainsKey(key))
        {
            _currentState = _states[key];
            await _currentState.Enter();
        }
        else
        {
            Debug.LogError("State not found");
        }
    }

    public State GetCurrentState()
    {
        return _currentState;
    }
}