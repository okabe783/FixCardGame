using UnityEngine;

[CreateAssetMenu(menuName = "Enemy")]
public class EnemySettings : ScriptableObject
{
    public EnemyAttribute EnemyAttribute;
    public int EnemyID;
    public int Hp;
    public Sprite Sprite;
}