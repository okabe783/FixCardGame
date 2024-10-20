using UnityEngine;

/// <summary>
/// Viewの管理
/// エフェクトやボイスなどを管理する
/// </summary>
public class InGameView : MonoBehaviour
{
    private void Start()
    {
        SetUp();
    }

    private async void SetUp()
    {
        //敵のAnimationをする
        //手札を配る
        await InGameLogic.I.AddCardToHand();
        //StartPanelをアクティブにする
        await InGameLogic.I.ActivePhasePanel("StartPhase");
        //Phaseのスタート
        InGameLogic.I.ChangePhaseState(InGamePhase.TurnStart);
    }
}
