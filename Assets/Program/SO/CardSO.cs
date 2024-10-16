using UnityEngine;

[CreateAssetMenu(fileName = "Card")]
public class CardSO : ScriptableObject
{
    public int ID;
    public Sprite Icon;
    public string Description;
}