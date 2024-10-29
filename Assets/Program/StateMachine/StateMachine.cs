using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] private InGameView _inGameView;
    [SerializeField] private Enemy _enemy;
    private State _currentState; 
    private readonly Dictionary<string, State> _states = new(); 
    private static StateMachine _stateMachine;

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
        AddState("mulligan", new MulliganPhase(_inGameView));
        AddState("turnStart", new TurnStartPhase(_inGameView));
        AddState("play", new PlayPhase());
        AddState("battle", new BattlePhase());
        AddState("turnEnd", new TurnEndPhase(_inGameView));
        AddState("gameEnd",new GameEnd(_inGameView,_enemy));
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
    
    public async UniTask ChangeState(string key)
    {
        if (_currentState != null)
        {
            _currentState.Exit();
        }

        if (_states.TryGetValue(key, out var state))
        {
            _currentState = state;
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