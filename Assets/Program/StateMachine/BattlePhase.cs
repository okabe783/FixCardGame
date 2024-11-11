using Cysharp.Threading.Tasks;

public class BattlePhase : State
{
    public override async UniTask Enter()
    {
        
    }

    public override async void Exit()
    {
        // 敵のスキルを発動する
        await InGameLogic.I.ActiveEnemySkill();
    }
}