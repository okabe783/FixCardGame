using Cysharp.Threading.Tasks;
using UnityEngine;

public class TurnStartPhase : State
{
    private readonly InGameView _view;

    public override async UniTask Enter(string panelName)
    {
        await　base.Enter(panelName);
        await StateMachine.GetInstance().ChangeState("play", "Main");
    }

    public TurnStartPhase(InGameView view)
    {
        _view = view;
    }

    public override void OnUpdate()
    {
        //リフレッシュの処理
    }

    public override void Exit()
    {
        Debug.Log("StartPhaseを抜けた");
    }
}