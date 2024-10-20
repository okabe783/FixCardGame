using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

//ToDo:StartでEnterを呼び出す
public class StateMachine
{
    private State _currentState; // 現在のState
    private Dictionary<string, State> _states = new(); // 全Phaseを管理する

    // 状態を登録
    public void AddState(string key, State state)
    {
        _states.TryAdd(key, state);
    }

    public async UniTask ChangeState(string key, string panelName)
    {
        if (_currentState != null)
        {
            _currentState.Exit(); // 現在の状態を終了
        }

        if (_states.ContainsKey(key))
        {
            _currentState = _states[key];
            await _currentState.Enter(panelName);
        }
        else
        {
            Debug.LogError("State not found");
        }
    }

    public void Update()
    {
        _currentState?.OnUpdate();
    }
}