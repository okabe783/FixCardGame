using Cysharp.Threading.Tasks;

public class TurnStartPhase : State
{
    private readonly InGameView _view;

    public override async UniTask Enter()
    {
        await _view.DrawCard();
        await StateMachine.GetInstance().ChangeState("play");
    }

    public TurnStartPhase(InGameView view)
    {
        _view = view;
    }

    public override void OnUpdate()
    {
        
    }

    public override void Exit()
    {
        
    }
}