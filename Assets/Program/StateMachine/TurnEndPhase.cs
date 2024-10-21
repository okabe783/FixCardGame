using Cysharp.Threading.Tasks;
using UnityEngine;

public class TurnEndPhase :State
{
    public override async UniTask Enter(string PanelName)
    {
        //敵の攻撃処理
        await base.Enter(PanelName);
        await StateMachine.GetInstance().ChangeState("turnStart", "Start");
    }

    public override void OnUpdate()
    {
        
    }

    
    public override async void Exit()
    {
        Debug.Log("EndPhaseを抜けた");
    }
}