using Cysharp.Threading.Tasks;

public class MulliganPhase : State
{
    private readonly InGameView _view;

    public MulliganPhase(InGameView view)
    {
        _view = view;
    }

    public override async UniTask Enter()
    {
        await _view.GameMainSetUp();
        //Mulliganの処理
        await StateMachine.GetInstance().ChangeState("play");
    }

    public override void OnUpdate()
    {
    }

    public override void Exit()
    {
        
    }
}