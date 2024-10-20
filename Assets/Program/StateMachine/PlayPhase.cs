using Cysharp.Threading.Tasks;

public class PlayPhase : State
{
    public override async UniTask Enter(string PanelName)
    {
        await base.Enter(PanelName);
    }

    public override void OnUpdate()
    {
        //Drag可能にする処理
    }

    //ToDo:カード側がUpdateでStateがPlayだったらDragを有効にする処理をもつ
    public override void Exit()
    {
        //次のStateに行く
        //Dragを無効にする
    }
}