using UnityEngine;

public class CardGenerator : MonoBehaviour
{
    [SerializeField, Header("Cardのprefab")] private Card _cardPrefab;
    [SerializeField,Header("Cardの移動をするStartPosition")] private GameObject _startCardPosition;

    /// <summary>Cardを生成する</summary>
    public Card SpawnCard(int cardID)
    {
        Card card = Instantiate(_cardPrefab,_startCardPosition.transform.position,Quaternion.identity);
        card.transform.SetParent(_startCardPosition.transform);
        card.SetCardData(cardID);
        return card;
    }
}