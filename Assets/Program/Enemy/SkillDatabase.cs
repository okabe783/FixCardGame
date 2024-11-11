using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillDatabase
{
    [SerializeField,SerializeReference,SubclassSelector,Header("敵のスキル")]
    private List<IAbility> _abilities = new();

    // Skillの発動処理
    public void ActiveSkill()
    {
        foreach (var ability in _abilities)
        {
            ability?.SetAbility();
        }
    }
}