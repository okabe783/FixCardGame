using Cysharp.Threading.Tasks;

public class TurnStartPhase : State
{
    public override async UniTask Enter(string panelName)
    {
        await base.Enter(panelName);
    }
    
    public override void OnUpdate()
    {
        //リフレッシュの処理
    }

    public override void Exit()
    {
        //次のStateに行く
    }
}