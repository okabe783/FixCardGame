using System.Collections.Generic;
using UnityEngine;

/// <summary>手札を管理するクラス</summary>
public class PlayerHand : MonoBehaviour
{
    //手札のcard情報
    private List<Card> _cards = new();

    //Cardを登録
    public void AddCard(Card card)
    {
        _cards.Add(card);
        //子にする
        card.transform.SetParent(transform);
    }

    public void RemoveCard(Card card)
    {
        _cards.Remove(card);
    }
    
    //手札をソートする
    public void ResetPosition() 
    {
        //小さい順に並べる
        //ToDo:UIDを渡してUID順にソートする
        _cards.Sort((card0, card1) => card0.CardDataBase.CardID - card1.CardDataBase.CardID);
        
        for (var i = 0; i < _cards.Count; i++)
        {
            var posX = i * 2f;
            _cards[i].transform.localPosition = new Vector2(posX, 0);
        }
    }
    
    //手札を削除する
    public void ResetCard()
    {
        foreach(var card in _cards)
        {
            Destroy(card.gameObject);
        }
        _cards.Clear();
    }
}
