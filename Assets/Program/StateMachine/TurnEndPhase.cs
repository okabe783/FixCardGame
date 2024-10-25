using Cysharp.Threading.Tasks;
using UnityEngine;

public class TurnEndPhase :State
{
    public override async UniTask Enter()
    {
        //敵の攻撃処理
        await StateMachine.GetInstance().ChangeState("turnStart");
    }

    public override void OnUpdate()
    {
        
    }

    
    public override void Exit()
    {
        
    }
}