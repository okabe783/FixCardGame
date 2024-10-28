using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private Image _icon;
    [SerializeField] private CanvasGroup _panelCanvasGroup;
    [SerializeField, Header("説明")]　private TextMeshProUGUI _descriptionText;

    private bool _isDraggable;

    private int _power;
    private string _attackEffectName;
    private string _summonEffectName;
    private int _id;
    private Vector2 _currentPosition;
    private const float _threshold = 50f;
    private EnemyAttribute _skill;
    public UnityAction OnEndDragAction;

    //勝敗を決めるときに使う
    public CardSO CardDataBase { get; private set; }

    public EnemyAttribute GetCardSkill()
    {
        return _skill;
    }

    public int GetCardPower()
    {
        return _power;
    }

    public string GetAttackEffectName()
    {
        return _attackEffectName;
    }

    public string GetSummonEffectName()
    {
        return _summonEffectName;
    }

    public CanvasGroup GetPanel()
    {
        return _panelCanvasGroup;
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

    private void Update()
    {
        if (StateMachine.GetInstance().GetCurrentState() is PlayPhase)
        {
            _isDraggable = true;
        }
        else
        {
            _isDraggable = false;
        }
    }

    public void CardSet(int cardID)
    {
        CardSO cardBase = Resources.Load<CardSO>("SOPrefabs/Card/Card" + cardID);
        CardDataBase = cardBase;
        _icon.sprite = cardBase.Icon;
        _descriptionText.text = cardBase.Description;
        _id = cardBase.ID;
        _power = cardBase.CardPower;
        _attackEffectName = cardBase.AttackEffectName;
        _summonEffectName = cardBase.SummonEffectName;

        //属性をCardSkillから取得して保持
        if (_id < CardSkill.AllAttributes.Count)
        {
            _skill = CardSkill.AllAttributes[_id - 1];
        }
        else
        {
            Debug.LogError("cardIDがAllAttributesの範囲外です");
        }
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
            float distance = Vector2.Distance(_currentPosition, eventData.position);
            if (distance > _threshold)
            {
                InGameLogic.I.PlayCard(this).Forget();
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
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)transform.parent, // 親のRectTransform
                eventData.position, // ドラッグ時のスクリーン座標
                eventData.pressEventCamera, // イベントカメラ
                out Vector2 localPoint); // ローカル座標に変換された結果を格納

            // ローカル座標に変換された座標を使ってカードの位置を更新
            transform.localPosition = localPoint;
        }
    }
}