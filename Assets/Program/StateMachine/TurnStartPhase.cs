using Cysharp.Threading.Tasks;

public class TurnStartPhase : IState
{
    public async UniTask Enter()
    {
        await InGameLogic.I.StartCard();
        await InGameLogic.I.ShowPhasePanel("StartPhase");
        await StateMachine.GetInstance().ChangeState("play");
    }

    public void Exit()
    {
        
    }
}