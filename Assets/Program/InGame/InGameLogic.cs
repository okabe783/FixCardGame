using System.Collections.Generic;
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
    public PlayerHand PlayerHand => _playerHand;

    private void Start()
    {
        _enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

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
    
    public async UniTask AddCardToHand()
    {
        List<int> cardIDs = Enumerable.Range(0, _cardDataList).ToList();
        cardIDs = cardIDs.OrderBy(_ => Random.value).ToList();

        for (int i = 0; i < 3; i++)
        {
            Card createCard = _cardGenerator.SpawnCard(cardIDs[i + 1]);
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
        PlayerHand.RemoveCard(card);
        await StateMachine.GetInstance().ChangeState("battle");
        await CardBattle(card);
    }

    private async UniTask CardBattle(Card card)
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

    private async UniTask PerformPlayerAttack(Card card)
    {
        // Effectの設定
        EffectSettings effectPrefab = Resources.Load<EffectSettings>("Motion/" + card.GetAttackEffectName());
        if (effectPrefab == null)
        {
            Debug.LogError("Resourcesが読み込めません");
            return;
        }
        
        var cutInAnimation = _inGameView.gameObject.GetComponent<CutInPanel>();
        await cutInAnimation.SlideIn(card.GetIcon(),"Player");
        _enemy.SetCurrentHp(card.GetCardPower());
        UpdateHPBars();
        EffectSettings effectInstance = Instantiate(effectPrefab, card.transform.position, Quaternion.identity);
        await effectInstance.MoveEffectToTarget(effectInstance, _enemy.transform.position);
        
        EffectSettings hitEffectPrefab = Resources.Load<EffectSettings>("Motion/" + effectPrefab.OnHitEffect);
        EffectSettings hitEffectInstance =
            Instantiate(hitEffectPrefab, _enemy.transform.position, Quaternion.identity);
        await hitEffectInstance.SetParticle(hitEffectInstance);
    }

    private async UniTask PerformEnemyAttack(Card card)
    {
        EffectSettings effectPrefab = Resources.Load<EffectSettings>("Motion/" + _enemy.GetEffectName());
        if (effectPrefab == null)
        {
            Debug.LogError("Resourcesが読み込めません");
            return;
        }
        
        var cutInAnimation = _inGameView.gameObject.GetComponent<CutInPanel>();
        await cutInAnimation.SlideIn(_enemy.GetIcon(),"Enemy");
        _player.ChangeHealth(1);
        UpdateHPBars();
        
        EffectSettings effectInstance = Instantiate(effectPrefab, _enemy.transform.position, Quaternion.identity);
        await effectInstance.MoveEffectToTarget(effectInstance, card.transform.position);
        
        EffectSettings hitEffectPrefab = Resources.Load<EffectSettings>("Motion/" + effectPrefab.OnHitEffect);
        EffectSettings hitEffectInstance = Instantiate(hitEffectPrefab, card.transform.position, Quaternion.identity);
        await hitEffectInstance.SetParticle(hitEffectInstance);
    }
}