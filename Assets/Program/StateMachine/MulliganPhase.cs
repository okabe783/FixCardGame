using Cysharp.Threading.Tasks;
using UnityEngine;

public class MulliganPhase : State
{
    private readonly InGameView _view;

    public MulliganPhase(InGameView view)
    {
        _view = view;
    }

    public override async UniTask Enter(string panelName)
    {
        await base.Enter(panelName); // パネルの表示
        await _view.GameMainSetUp();
        //Mulliganの処理
        await StateMachine.GetInstance().ChangeState("play", "Main");
    }

    public override void OnUpdate()
    {
    }

    public override void Exit()
    {
        //次のStateに行く
        Debug.Log("MulliganPhaseを抜けた");
    }
}