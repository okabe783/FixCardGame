using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    private IState _currentState;
    private readonly Dictionary<string, IState> _states = new();
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
        AddState("mulligan", new MulliganPhase());
        AddState("turnStart", new TurnStartPhase());
        AddState("play", new PlayPhase());
        AddState("battle", new BattlePhase());
        AddState("turnEnd", new TurnEndPhase());
        AddState("gameEnd", new GameEnd(_enemy));
    }

    private async UniTask Start()
    {
        await ChangeState("mulligan");
    }

    // 状態を登録
    private void AddState(string key, IState state)
    {
        _states.TryAdd(key, state);
    }

    public async UniTask ChangeState(string key, Card currentCard = null)
    {
        if (_currentState != null)
        {
            _currentState.Exit();
        }

        if (_states.TryGetValue(key, out IState state))
        {
            // バトルstateだった場合カードをセット
            if (state is BattlePhase battlePhase && currentCard != null)
            {
                battlePhase.SetCard(currentCard);
            }

            _currentState = state;
            await _currentState.Enter();
        }
        else
            Debug.LogError("Stateが存在しません");
    }
}