using UnityEngine;

[CreateAssetMenu(fileName = "Card")]
public class CardSO : ScriptableObject
{
    public string Name;
    public int CardID;
    public int Power;
    public Sprite Icon;
    public string Description;

    public int[] advantageousAttribute;
}
