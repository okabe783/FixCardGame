using Cysharp.Threading.Tasks;

public class TurnStartPhase : IState
{
    public async UniTask Enter()
    {
        await InGameSystem.I.StartCard();
        await InGameSystem.I.ShowPhasePanel("StartPhase");
        await StateMachine.GetInstance().ChangeState("play");
    }

    public void Exit()
    {
        
    }
}