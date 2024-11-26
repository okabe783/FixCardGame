using UnityEngine;

public class CardSettings
{
    private CardSO[] _cardData;

    // CardDataを取得する
    public CardSO GetCardData(int index)
    {
        return _cardData[index];
    }

    // Cardの読み込み
    public void Init(int cardCount)
    {
        _cardData = new CardSO[cardCount];
        for (int i = 1; i <= cardCount; i++)
        {
            CardSO cardData = Resources.Load<CardSO>($"SOPrefabs/Card/Card{i}");
            
            if (cardData == null)
            {
                Debug.LogError($"CardDataが存在しません");
                return;
            }
            
            _cardData[i - 1] = cardData;
        }
    }
}