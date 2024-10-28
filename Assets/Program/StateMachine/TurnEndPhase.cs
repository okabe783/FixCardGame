using Cysharp.Threading.Tasks;

public class TurnEndPhase :State
{
    private readonly InGameView _view;

    public TurnEndPhase (InGameView view)
    {
        _view = view;
    }
    
    public override async UniTask Enter()
    {
        // ToDo:敵の攻撃処理
        await _view.ShowActivePanel("EndPhase");
        await StateMachine.GetInstance().ChangeState("turnStart");
    }

    
    public override void Exit()
    {
        
    }
}