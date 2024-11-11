using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
public class SkillDatabase
{
    [SerializeField,SerializeReference,SubclassSelector,Header("敵のスキル")]
    private List<IAbility> _abilities = new();

    // Skillの発動処理
    public async UniTask ActiveSkill(Enemy enemy)
    {
        var task = new List<UniTask>();
        {
            foreach (var ability in _abilities)
            {
                task.Add(ability.SetAbility(enemy));
            }
        }

        await UniTask.WhenAll(task);
    }
}