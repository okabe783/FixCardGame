using UnityEngine;

public class PlayPhase : State
{
    public override void OnUpdate()
    {
        //Drag可能にする処理
    }
    
    public override void Exit()
    {
        //Dragを無効にする
        Debug.Log("PlayPhaseを抜けた");
    }
}