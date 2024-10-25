using UnityEngine;

[CreateAssetMenu(fileName = "Card")]
public class CardSO : ScriptableObject
{
    public string AttackEffectName;
    public string SummonEffectName;
    public int CardPower;
    public int ID;
    public Sprite Icon;
    public string Description;
}