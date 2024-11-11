using UnityEngine;

[CreateAssetMenu(menuName = "Enemy")]
public class EnemySettings : ScriptableObject
{
    public int EnemyID;
    public int Hp;
    public Sprite Sprite;
    public string EffectName;

    [SerializeField] private SkillDatabase _commandDatabase;
}