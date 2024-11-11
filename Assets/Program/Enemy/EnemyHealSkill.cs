using UnityEngine;

public class EnemyHealSkill : IAbility
{
    private IAbility _abilityImplementation;
    public string Name() => "回復スキル";

    public void SetAbility()
    {
        Debug.Log("体力を回復した");
    }
}