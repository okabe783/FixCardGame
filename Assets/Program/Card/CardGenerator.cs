using UnityEngine;

/// <summary>カードを配る</summary>
public class CardGenerator : MonoBehaviour
{
    [SerializeField, Header("カードのデータ")] private CardSO[] _cardData;

    [SerializeField, Header("Cardのprefab")] private Card _cardPrefab;
    [SerializeField,Header("Cardの移動をするStartPosition")] private GameObject _startCardPosition;

    /// <summary>Cardを生成する</summary>
    public Card SpawnCard()
    {
        Card card = Instantiate(_cardPrefab,_startCardPosition.transform.position,Quaternion.identity);
        card.CardSet(_cardData[Random.Range(0, _cardData.Length)]);
        return card;
    }
}