using Cysharp.Threading.Tasks;

public class EnemyHealSkill : IAbility
{
    public string Name() => "回復スキル";

    public async UniTask SetAbility(Enemy enemy)
    {
        // 回復の処理
        enemy.SetCurrentHp(-enemy.GetHealValue());
        // ToDo: Effectの処理
        await UniTask.Delay(500); 
    }
}