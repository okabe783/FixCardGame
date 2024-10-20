using Cysharp.Threading.Tasks;

public class MulliganPhase :State
{
    public override async UniTask Enter(string PanelName)
    {
        await base.Enter(PanelName);
    }

    public override void OnUpdate()
    {
        //Mulliganの処理
    }

    public override void Exit()
    {
        //次のStateに行く
    }
}