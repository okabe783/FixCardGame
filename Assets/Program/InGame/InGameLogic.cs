using System.Linq;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

//　InGameの機構を管理する
public class InGameLogic : SingletonMonoBehaviour<InGameLogic>
{
    [SerializeField] private InGameView _inGameView;
    [SerializeField] private CardGenerator _cardGenerator;
    [SerializeField, Header("手札")] private PlayerHand _playerHand;
    [SerializeField, Header("Playする位置")] private GameObject _targetTransform;
    // ToDo : CardGeneratorにもある
    [SerializeField, Header("全てのCardの数")] private int _cardDataList;

    private int _turnCount;

    private Player _player;
    private Enemy _enemy;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();

        if (_enemy == null || _player == null)
        {
            Debug.LogError("取得に失敗しました");
            return;
        }

        UpdateHpBars();
        _cardGenerator.SetCardData();
    }

    private void UpdateHpBars()
    {
        _inGameView.ChangeHPBar(_player.GetHP(), 0);
        _inGameView.ChangeHPBar(_enemy.GetCurrentHp(), 1);
    }

    /// <summary>GameMain突入時に呼び出される</summary>
    public async UniTask GameMainSetUp()
    {
        await StartCard();
        await _inGameView.ShowActivePanel("StartPhase");
    }

    public async UniTask StartCard()
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
            createCard.OnBattleAction += async () => await StateMachine.GetInstance().ChangeState("battle",createCard);
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
        await _inGameView.ShowEffect(card.GetSummonEffectName(), _targetTransform.transform.position,false);
        card.GetPanel().alpha = 1;
        _playerHand.RemoveCard(card);
    }

    public async UniTask CardBattle(Card card)
    {
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

    public async UniTask ActiveEnemySkill()
    {
        _turnCount++;

        if (_turnCount == _enemy.GetActiveSkillTurn())
        {
            await _enemy.ActiveSkill(_enemy);
            _turnCount = 0;
        }
    }

    // Playerの攻撃が成功したときの処理
    private async UniTask PerformPlayerAttack(Card card)
    {
        await _inGameView.ShowCutInAnimation(card.GetIcon(), "Player");
        _enemy.SetCurrentHp(card.GetCardPower());
        UpdateHpBars();
        await _inGameView.ShowEffect(card.GetAttackEffectName(), card.transform.position, _enemy.transform.position,false);
    }

    // Enemyの攻撃が成功したときの処理
    private async UniTask PerformEnemyAttack(Card card)
    {
        await _inGameView.ShowCutInAnimation(_enemy.GetIcon(), "Enemy");
        _player.ChangeHealth(_enemy.GetPower());
        UpdateHpBars();
        await _inGameView.ShowEffect(_enemy.GetEffectName(), _enemy.transform.position, card.transform.position,true);
    }

    // CardのDrag処理
    public void SetCardsDraggable(bool draggable)
    {
        foreach (Card card in _playerHand.GetAllCards())
        {
            card.IsDragging(draggable);
        }
    }
}