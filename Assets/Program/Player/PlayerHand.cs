using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

/// <summary>手札を管理するクラス</summary>
public class PlayerHand : MonoBehaviour
{
    //手札のcard情報
    private List<Card> _cards = new();

    public List<Card> GetAllCards() => _cards;

    //Cardを登録
    public async UniTask AddCard(Card card)
    {
        _cards.Add(card);
        await card.transform.DOMove(transform.position, 0.5f);
        card.transform.SetParent(transform);
        ResetPosition();
        //手札をソートをする関数を登録する
        card.OnEndDragAction += ResetPosition;
    }

    public void RemoveCard(Card card)
    {
        _cards.Remove(card);
    }

    //手札をソートする
    private void ResetPosition()
    {
        for (int i = 0; i < _cards.Count; i++)
        {
            float center = (_cards.Count - 1) / 2.0f;
            float interval = 15.0f;
            float x = (i - center) * interval;
            _cards[i].transform.localPosition = new Vector3(x, 0, 0);
        }
    }

    //手札を削除する
    public void ResetCard()
    {
        foreach (var card in _cards)
        {
            Destroy(card.gameObject);
        }

        _cards.Clear();
    }
}