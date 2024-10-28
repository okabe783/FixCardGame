using Cysharp.Threading.Tasks;

public class TurnStartPhase : State
{
    private readonly InGameView _view;
    
    public TurnStartPhase(InGameView view)
    {
        _view = view;
    }

    public override async UniTask Enter()
    {
        await _view.DrawCard();
        await _view.ShowActivePanel("StartPhase");
        await StateMachine.GetInstance().ChangeState("play");
    }

    public override void Exit()
    {
        
    }
}