using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
public class EnemyPowerUpSkill : IAbility
{
    public string Name()　=> "攻撃力上昇スキル";
    public int ActiveTurn { get; set; }

    public async UniTask SetAbility(Enemy enemy)
    {
        Debug.Log($"{enemy.GetPower()}から");
        enemy.SetPower();
        Debug.Log($"{enemy.GetPower()}になりました");
        // ToDo : スキルのエフェクトを発動
        await UniTask.Delay(500); 
    }
}