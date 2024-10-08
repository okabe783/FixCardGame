using UnityEngine;

/// <summary>カードを配る</summary>
public class CardGenerator : MonoBehaviour
{
    [SerializeField, Header("カードのデータ")] private CardSO[] _cardData;

    [SerializeField, Header("Cardのprefab")] private Card _cardPrefab;

    /// <summary>Cardを生成する</summary>
    public Card SpawnCard()
    {
        Card card = Instantiate(_cardPrefab);
        card.CardSet(_cardData[Random.Range(0, _cardData.Length)]);
        return card;
    }
}