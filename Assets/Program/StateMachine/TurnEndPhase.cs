using Cysharp.Threading.Tasks;

public class TurnEndPhase :IState
{
    
    public async UniTask Enter()
    {
        await InGameSystem.I.ShowPhasePanel("EndPhase");
        await StateMachine.GetInstance().ChangeState("turnStart");
    }

    
    public void Exit()
    {
        
    }
}