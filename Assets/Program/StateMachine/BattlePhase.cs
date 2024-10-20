using Cysharp.Threading.Tasks;

namespace Program.StateMachine
{
    public class BattlePhase :State
    {
        public override async UniTask Enter(string PanelName)
        {
            await base.Enter(PanelName);
        }

        public override void OnUpdate()
        {
            //Battle処理
        }
        
        public override void Exit()
        {
            //次のStateに行く
        }
    }
}