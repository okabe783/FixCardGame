using Cysharp.Threading.Tasks;

public class TurnEndPhase :State
{
    
    public override async UniTask Enter()
    {
        await InGameLogic.I.ShowPhasePanel("EndPhase");
        await StateMachine.GetInstance().ChangeState("turnStart");
    }

    
    public override void Exit()
    {
        
    }
}