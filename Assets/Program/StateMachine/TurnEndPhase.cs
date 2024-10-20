using Cysharp.Threading.Tasks;

public class TurnEndPhase :State
{
    public override async UniTask Enter(string PanelName)
    {
        await base.Enter(PanelName);
    }

    public override void OnUpdate()
    {
        //敵の攻撃処理
    }

    
    public override void Exit()
    {
        //TurnStartに行く
    }
}