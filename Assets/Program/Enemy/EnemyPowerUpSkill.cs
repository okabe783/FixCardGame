using Cysharp.Threading.Tasks;

public class EnemyPowerUpSkill : IAbility
{
    public string Name()　=> "攻撃力上昇スキル";

    public async UniTask SetAbility(Enemy enemy)
    {
        enemy.SetPower();
        // ToDo : スキルのエフェクトを発動
        await UniTask.Delay(500); // エフェクトを待つ例
    }
}