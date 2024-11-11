using Cysharp.Threading.Tasks;

public class TurnStartPhase : State
{

    public override async UniTask Enter()
    {
        await InGameLogic.I.StartCard();
        await InGameLogic.I.ShowPhasePanel("StartPhase");
        await StateMachine.GetInstance().ChangeState("play");
    }

    public override void Exit()
    {
        
    }
}