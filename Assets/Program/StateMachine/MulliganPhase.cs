using Cysharp.Threading.Tasks;
using UnityEngine;

public class MulliganPhase : IState
{
    public async UniTask Enter()
    {
        await InGameLogic.I.GameMainSetUp();
        // ToDo:Mulliganの処理
        //OnUpdateをメソッド名を変更してそこに書く
        await StateMachine.GetInstance().ChangeState("play");
    }

    public void Exit()
    {
        
    }
}