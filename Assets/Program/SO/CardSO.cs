using UnityEngine;

[CreateAssetMenu(fileName = "Card")]
public class CardSO : ScriptableObject
{
    public string EffectName;
    public int CardPower;
    public int ID;
    public Sprite Icon;
    public string Description;
}