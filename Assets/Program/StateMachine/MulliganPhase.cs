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
        // ToDo:Mulliganの処理
        //OnUpdateをメソッド名を変更してそこに書く
        await StateMachine.GetInstance().ChangeState("play");
    }

    public override void Exit()
    {
        
    }
}