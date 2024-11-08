using System.Linq;
using UniRx;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

//　InGameの機構を管理する
public class InGameLogic : SingletonMonoBehaviour<InGameLogic>
{
    [SerializeField] private InGameView _inGameView;
    [SerializeField] private CardGenerator _cardGenerator;
    [SerializeField, Header("Cardを配る場所")] private PlayerHand _playerHand;
    [SerializeField, Header("置きたい場所")] private GameObject _targetTransform;
    [SerializeField, Header("全てのCardの数")] private int _cardDataList;

    private Player _player;
    private Enemy _enemy;
   
    private readonly ReactiveProperty<InGamePhase> _currentPhase = new();
    public IReactiveProperty<InGamePhase> CurrentPhase => _currentPhase;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();

        if (_enemy == null || _player == null)
        {
            Debug.LogError("取得に失敗しました");
            return;
        }

        UpdateHPBars();
    }

    private void UpdateHPBars()
    {
        _inGameView.ChangeHPBar(_player.GetHP(), 1);
        _inGameView.ChangeHPBar(_enemy.GetCurrentHp(), 0);
    }
    
    public async UniTask GameMainSetUp()
    {
        await DrawCard();
        await _inGameView.ShowActivePanel("StartPhase");
    }

    public async UniTask DrawCard()
    {
        _playerHand.ResetCard();
        await AddCardToHand();
    }
    
    private async UniTask AddCardToHand()
    {
        if (_cardDataList < 3)
        {
            Debug.LogError("カードデータの数が足りません。");
            return;
        }
        int[] cardIDs = Enumerable.Range(0, _cardDataList).OrderBy(_ => Random.value).Take(3).ToArray();

        for (int i = 0; i < 3; i++)
        {
            Card createCard = _cardGenerator.SpawnCard(cardIDs[i]);
            await _playerHand.AddCard(createCard);
        }
    }

    public async UniTask ShowPhasePanel(string phaseName)
    {
        await _inGameView.ShowActivePanel(phaseName);
    }

    public async UniTask ShowGameEndPanel(string panelText)
    {
        await _inGameView.ShowGameEndPanel(panelText);
    }
    
    public async UniTask PlayCard(Card card)
    {
        card.GetPanel().alpha = 0;
        await card.transform.DOMove(_targetTransform.transform.position, 0.1f);
        await _inGameView.ShowEffect(card.GetSummonEffectName(), _targetTransform.transform.position);
        card.GetPanel().alpha = 1;
        _playerHand.RemoveCard(card);
        await StateMachine.GetInstance().ChangeState("battle");
        await CardBattle(card);
    }

    private async UniTask CardBattle(Card card)
    {
        Debug.Log(_enemy.GetAttribute());
        bool isEffective = (_enemy.GetAttribute() & card.GetCardSkill()) != 0;

        if (isEffective)
        {
            await PerformPlayerAttack(card);
        }
        else
        {
            await PerformEnemyAttack(card);
        }
        
        if (_enemy.GetCurrentHp() <= 0 || _player.GetHP() <= 0)
        {
            await StateMachine.GetInstance().ChangeState("gameEnd");
            return;
        }
        
        Destroy(card.gameObject);
        await StateMachine.GetInstance().ChangeState("turnEnd");
    }

    private async UniTask PerformPlayerAttack(Card card)
    {
        await _inGameView.ShowCutInAnimation(card.GetIcon(), "Player");
        _enemy.SetCurrentHp(card.GetCardPower());
        UpdateHPBars();
        await _inGameView.ShowEffect(card.GetAttackEffectName(), card.transform.position,_enemy.transform.position);
    }

    private async UniTask PerformEnemyAttack(Card card)
    {
        await _inGameView.ShowCutInAnimation(_enemy.GetIcon(), "Enemy");
        _player.ChangeHealth(1);
        UpdateHPBars();
        await _inGameView.ShowEffect(_enemy.GetEffectName(), _enemy.transform.position,card.transform.position);
    }
}