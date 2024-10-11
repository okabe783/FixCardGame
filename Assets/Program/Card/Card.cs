using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IDragHandler
{
    [SerializeField, Header("Image")] private Image _icon;
    [SerializeField, Header("説明")]　private TextMeshProUGUI _descriptionText;
    [SerializeField] private bool _isDraggable;
    [SerializeField] private float _floatingAmount = 0.1f;
    [SerializeField] private float _rotationFactor = 1;

    private Vector3 _currentPosition;
    private const float _threshold = 50f;
    public UnityAction OnEndDragAction; //Drag終了時に実行したい関数を登録

    //勝敗を決めるときに使う
    public CardSO CardDataBase { get; private set; }

    public bool IsDraggable
    {
        set => _isDraggable = value;
    }

    private void Start()
    {
        InGameLogic.I.CurrentPhase.Subscribe(phase =>
        {
            if (phase == InGamePhase.Play)
            {
                _isDraggable = true;
            }
            else
            {
                _isDraggable = false;
            }
            
        }).AddTo(this);
    }
    public void CardSet(CardSO cardBase)
    {
        CardDataBase = cardBase;
        _icon.sprite = cardBase.Icon;
        _descriptionText.text = cardBase.Description;
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
        //Cardの移動
        if (_isDraggable)
        {
            float distance = Vector3.Distance(_currentPosition, eventData.position);
            if (distance > _threshold)
            {
                InGameLogic.I.PlayCard(this);
            }
            else
            {
                OnEndDragAction?.Invoke();
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isDraggable)
        {
            transform.position = eventData.position;
        }
    }
}