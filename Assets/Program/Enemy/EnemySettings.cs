using UnityEngine;

[CreateAssetMenu(menuName = "Enemy")]
public class EnemySettings : ScriptableObject
{
    public int EnemyID;
    public int ActiveSkillTurn;
    public int Hp;
    public Sprite Sprite;
    public string EffectName;
    public int powerValue;
    public int HealValue;

    [SerializeField] private SkillDatabase _commandDatabase;

    public SkillDatabase CommandDatabase => _commandDatabase;
}