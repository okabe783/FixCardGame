using Cysharp.Threading.Tasks;


public class MulliganPhase : IState
{
    public async UniTask Enter()
    {
        await InGameLogic.I.GameMainSetUp();
        // ToDo:Mulliganの処理
        await StateMachine.GetInstance().ChangeState("play");
    }

    public void Exit()
    {
        
    }
}