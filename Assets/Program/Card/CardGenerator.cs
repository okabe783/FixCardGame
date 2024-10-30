using UnityEngine;

public class CardGenerator : MonoBehaviour
{
    [SerializeField, Header("Cardのprefab")] private Card _cardPrefab;
    [SerializeField,Header("Cardの移動をするStartPosition")] private GameObject _startCardPosition;
    [SerializeField,Header("CardDataの数")]private int _cardDataList = 3;

    /// <summary>Cardを生成する</summary>
    public Card SpawnCard()
    {
        Card card = Instantiate(_cardPrefab,_startCardPosition.transform.position,Quaternion.identity);
        card.transform.SetParent(_startCardPosition.transform);
        card.CardSet(Random.Range(1, _cardDataList + 1));
        return card;
    }
}