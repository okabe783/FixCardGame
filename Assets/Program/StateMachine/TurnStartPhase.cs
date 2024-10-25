using Cysharp.Threading.Tasks;
using UnityEngine;

public class TurnStartPhase : State
{
    private readonly InGameView _view;

    public override async UniTask Enter()
    {
        await StateMachine.GetInstance().ChangeState("play");
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
        
    }
}