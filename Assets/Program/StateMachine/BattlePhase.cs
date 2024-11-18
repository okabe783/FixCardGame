using Cysharp.Threading.Tasks;

public class BattlePhase : IState
{
    public UniTask Enter()
    {
        return UniTask.CompletedTask;
    }

    public async void Exit()
    {
        // 敵のスキルを発動する
        await InGameLogic.I.ActiveEnemySkill();
    }
}