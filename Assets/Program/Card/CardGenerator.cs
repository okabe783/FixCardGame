using UnityEngine;

public class CardGenerator : MonoBehaviour
{
    [SerializeField] private Card _cardPrefab;
    [SerializeField,Header("CardAnimationのスタート位置")] private GameObject _startCardPosition;
    private CardSettings _cardSettings;

    public void SetCardData()
    {
        _cardSettings = new CardSettings();
        
        if (_cardSettings == null)
        {
            Debug.LogError("CardDataがセットされていません");
        }
        
        _cardSettings.Init(CardAttribute.AllAttributes.Count);
    }

    /// <summary>Cardを生成する</summary>
    public Card SpawnCard(int cardID)
    {
        Card card = Instantiate(_cardPrefab,_startCardPosition.transform.position,Quaternion.identity);
        card.transform.SetParent(_startCardPosition.transform);
        CardSO cardData =  _cardSettings.GetCardData(cardID);
        card.SetCardData(cardData);
        return card;
    }
}