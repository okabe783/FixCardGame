using Cysharp.Threading.Tasks;

public class MulliganPhase : State
{
    public override async UniTask Enter()
    {
        await InGameLogic.I.GameMainSetUp();
        // ToDo:Mulliganの処理
        //OnUpdateをメソッド名を変更してそこに書く
        await StateMachine.GetInstance().ChangeState("play");
    }

    public override void Exit()
    {
        
    }
}