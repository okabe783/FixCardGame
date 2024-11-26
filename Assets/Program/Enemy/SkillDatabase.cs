using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
public class SkillDatabase
{
    [SerializeField]
    private List<SkillData> _skillDataList = new();
    
    public List<SkillData> SkillDataList => _skillDataList;
    
    // Skillの発動処理
    public async UniTask ActiveSkill(Enemy enemy)
    {
        List<UniTask> task = new();
        {
            foreach (var skillData  in _skillDataList)
            {
                task.Add(skillData.Ability.SetAbility(enemy));
            }
        }

        await UniTask.WhenAll(task);
    }
}