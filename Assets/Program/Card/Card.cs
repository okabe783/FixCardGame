using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField, Header("Image")] private Image _icon;
    [SerializeField, Header("説明")]　private TextMeshProUGUI _descriptionText;

    //勝敗を決めるときに使う
    public CardSO CardDataBase { get; private set; } 

    public void CardSet(CardSO cardBase)
    {
        CardDataBase = cardBase;
        _icon.sprite = cardBase.Icon;
        _descriptionText.text = cardBase.Description;
    }
}