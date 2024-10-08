using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Cardの移動を管理する</summary>
public class CardManager : SingletonMonoBehaviour<CardManager>
{
   [SerializeField,Header("手札の位置")] private PlayerHand _homeTransform;
   [SerializeField,Header("置きたい場所")] private GameObject _targetTransform;

   public void PlayCard(Card card)
   {
      //Cardをターゲットにセットする
      _homeTransform.RemoveCard(card);
      card.transform.position = _targetTransform.transform.position;
      //ToDo:Phaseで値を管理
      card.IsDraggable = false;
   }
}