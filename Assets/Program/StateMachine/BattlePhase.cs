using Cysharp.Threading.Tasks;
using UnityEngine;


public class BattlePhase : State
{
    public override async UniTask Enter()
    {
        
    }

    public override void OnUpdate()
    {
        //Battle処理
    }

    public override async void Exit()
    {
        //次のStateに行く
        Debug.Log("PlayPhaseを抜けた");
    }
}