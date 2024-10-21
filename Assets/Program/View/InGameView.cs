using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Viewの管理
/// エフェクトやボイスなどを管理する
/// </summary>
public class InGameView : MonoBehaviour
{
    public async UniTask GameMainSetUp()
    {
        //敵のAnimationをする
        //手札を配る
        await InGameLogic.I.AddCardToHand();
        //StartPanelをアクティブにする
        await InGameLogic.I.ActivePhasePanel("StartPhase");
    }

    // public async UniTask Battle()
    // {
    //     await InGameLogic.I.CardBattle();
    //     await StateMachine.GetInstance().ChangeState("turnEnd","turnEnd");
    // }
}
