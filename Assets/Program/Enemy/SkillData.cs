using System;
using UnityEngine;

[Serializable]
public class SkillData
{
    [SerializeReference, SubclassSelector]
    public IAbility Ability; // スキル本体

    [Header("アクティブスキルターン")]
    public int ActiveTurn; // このスキルが発動するターン
}
