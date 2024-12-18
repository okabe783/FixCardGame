using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Card : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler,IDraggable
{
    [SerializeField] private Image _icon;
    [SerializeField] private CanvasGroup _panelCanvasGroup;
    [SerializeField, Header("説明")]　private TextMeshProUGUI _descriptionText;
    [SerializeField] private Image[] _colorPanelObject;
    
    private int _power;
    private int _id;
    
    private const float _threshold = 200f;
    private string _attackEffectName;
    private string _summonEffectName;
    private Vector2 _currentPosition;
    private EnemyAttribute _skill;
    private Dictionary<EnemyAttribute, Color> _attributeColors;
    
    private bool _isDraggable;
    
    public UnityAction OnEndDragAction;
    public Func<UniTask> OnBattleAction;

    #region 外部に公開するステータス
    public EnemyAttribute GetCardSkill() => _skill;

    public int GetCardPower() => _power;

    public string GetAttackEffectName() => _attackEffectName;

    public string GetSummonEffectName() => _summonEffectName;

    public CanvasGroup GetPanel() => _panelCanvasGroup;

    public Image GetIcon() => _icon;
    
    #endregion

    private void Start()
    {
        InitializeAttributeColors();
        ApplyAttributeColors();
    }

    private void InitializeAttributeColors()
    {
        // 属性とそれに対応したカラーを辞書に登録
        _attributeColors = new Dictionary<EnemyAttribute, Color>
        {
            { EnemyAttribute.Red, Color.red },
            { EnemyAttribute.Green, Color.green },
            { EnemyAttribute.Blue, Color.blue },
            { EnemyAttribute.Yellow, Color.yellow },
            { EnemyAttribute.Black, Color.black },
            { EnemyAttribute.White, Color.white }
        };
    }

    private void ApplyAttributeColors()
    {
        int colorIndex = 0;

        // 各属性Panelに対して属性をチェックし、色を設定
        foreach (KeyValuePair<EnemyAttribute, Color> attributeColor in _attributeColors)
        {
            if (_skill.HasFlag(attributeColor.Key) && colorIndex < _colorPanelObject.Length)
            {
                Image panel = _colorPanelObject[colorIndex];
                
                if (panel != null)
                {
                    //該当する色を設定
                    Color color = attributeColor.Value;
                    color.a = 1f;
                    panel.color = color;
                    colorIndex++;
                }
            }

            if (colorIndex >= _colorPanelObject.Length)
            {
                break;
            }
        }
    }

    public void SetCardData(CardSO cardData)
    {
        _icon.sprite = cardData.Icon;
        _descriptionText.text = cardData.Description;
        _id = cardData.ID;
        _power = cardData.CardPower;
        _attackEffectName = cardData.AttackEffectName;
        _summonEffectName = cardData.SummonEffectName;

        // 属性をCardSkillから取得して保持
        _skill = _id <= CardAttribute.AllAttributes.Count ? 
         CardAttribute.AllAttributes[_id - 1] : throw new ArgumentOutOfRangeException(nameof(cardData));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_isDraggable)
        {
            _currentPosition = eventData.position;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // カードの移動
        if (!_isDraggable) return;
        
        float distance = Vector2.Distance(_currentPosition, eventData.position);
        if (distance > _threshold)
        {
            // カードを提出したことを通知する
            OnBattleAction?.Invoke();
        }
        else
        {
            OnEndDragAction?.Invoke();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDraggable) return;
        
        // スクリーン座標からローカル座標に変換する
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)transform.parent, 
            eventData.position, // ドラッグ時のスクリーン座標
            eventData.pressEventCamera, // イベントカメラ
            out Vector2 localPoint); // ローカル座標に変換された結果を格納

        // ローカル座標に変換された座標を使ってカードの位置を更新
        transform.localPosition = localPoint;
    }

    // Drag制御
    public void IsDragging(bool isDragging)
    {
        _isDraggable = isDragging;
    }
}