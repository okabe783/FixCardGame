using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyHealSkill : IAbility
{
    public string Name() => "回復スキル";
    [SerializeField] private int _activeSkillTurn;
    public int ActiveTurn { get; set; }

    public async UniTask SetAbility(Enemy enemy)
    {
        // 回復の処理
        Debug.Log($"{enemy.GetCurrentHp()}から");
        enemy.SetCurrentHp(-enemy.GetHealValue());
        Debug.Log($"{enemy.GetCurrentHp()}になりました");
        // ToDo: Effectの処理
        await UniTask.Delay(500); 
    }
}