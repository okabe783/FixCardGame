using Cysharp.Threading.Tasks;

public class BattlePhase : IState
{
    public async UniTask Enter()
    {
        
    }

    public async void Exit()
    {
        // 敵のスキルを発動する
        await InGameLogic.I.ActiveEnemySkill();
    }
}