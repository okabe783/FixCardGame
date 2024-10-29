using UniRx;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class InGameLogic : SingletonMonoBehaviour<InGameLogic>
{
    [SerializeField] private Player _player;
    [SerializeField, Header("Cardを配る場所")] private PlayerHand _playerHand;
    [SerializeField, Header("置きたい場所")] private GameObject _targetTransform;
    [SerializeField] private InGameView _inGameView;
    
    private Enemy _enemy;
    private CardGenerator _cardGenerator;

    // Eventの発行
    private readonly ReactiveProperty<InGamePhase> _currentPhase = new();
    public IReactiveProperty<InGamePhase> CurrentPhase => _currentPhase;
    public PlayerHand PlayerHand => _playerHand;

    private void Start()
    {
        _enemy = FindAnyObjectByType<Enemy>();

        if (_enemy == null)
        {
            Debug.LogError("敵がみつかりません");
            return;
        }

        _cardGenerator = FindAnyObjectByType<CardGenerator>();

        if (_cardGenerator == null)
        {
            Debug.LogError("CardGeneratorが見つかりません");
        }

        _inGameView.ChangeHPBar(_enemy.GetCurrentHp(), 0);
        _inGameView.ChangeHPBar(_player.GetHP(), 1);
    }

    public async UniTask PlayCard(Card card)
    {
        card.GetPanel().alpha = 0;
        // Cardをターゲットにセットする
        await card.transform.DOMove(_targetTransform.transform.position, 0.1f);
        await _inGameView.ShowEffect(card.GetSummonEffectName(), _targetTransform.transform.position);
        card.GetPanel().alpha = 1;
        PlayerHand.RemoveCard(card);
        await StateMachine.GetInstance().ChangeState("battle");
        await CardBattle(card);
    }

    // 手札を配布する
    public async UniTask AddCardToHand()
    {
        for (int i = 0; i < 3; i++)
        {
            Card createCard = _cardGenerator.SpawnCard();
            await _playerHand.AddCard(createCard);
        }
    }

    public async UniTask CardBattle(Card card)
    {
        EnemyAttribute enemyAttribute = _enemy.GetAttribute();
        EnemyAttribute cardAttribute = card.GetCardSkill();

        if ((enemyAttribute & cardAttribute) != 0)
        {
            _enemy.SetCurrentHp(card.GetCardPower());
            _inGameView.ChangeHPBar(_enemy.GetCurrentHp(), 0);
            //Effectの設定
            EffectSettings effectPrefab = Resources.Load<EffectSettings>("Motion/" + card.GetAttackEffectName());
            EffectSettings effectInstance = Instantiate(effectPrefab, card.transform.position, Quaternion.identity);
            await effectInstance.MoveEffectToTarget(effectInstance, _enemy.transform.position);
            var hitEffectPrefab = Resources.Load<EffectSettings>("Motion/" + effectPrefab.OnHitEffect);
            EffectSettings hitEffectInstance =
                Instantiate(hitEffectPrefab, _enemy.transform.position, Quaternion.identity);
            await hitEffectInstance.SetParticle(hitEffectInstance);
        }
        else
        {
            var effectPrefab = Resources.Load<EffectSettings>("Motion/" + _enemy.GetEffectName());
            if (effectPrefab == null)
            {
                Debug.LogError("Resourcesが読み込めません");
                return;
            }

            //PlayerのHPを減らす
            _player.ChangeHealth(1);
            _inGameView.ChangeHPBar(_player.GetHP(), 1);
            EffectSettings effectInstance = Instantiate(effectPrefab, _enemy.transform.position, Quaternion.identity);
            await effectInstance.MoveEffectToTarget(effectInstance, card.transform.position);
            var hitEffectPrefab = Resources.Load<EffectSettings>("Motion/" + effectPrefab.OnHitEffect);
            var hitEffectInstance = Instantiate(hitEffectPrefab, card.transform.position, Quaternion.identity);
            await hitEffectInstance.SetParticle(hitEffectInstance);
        }

        if (_enemy.GetCurrentHp() <= 0 || _player.GetHP() <= 0)
        {
            // _gameEndPanel.gameObject.SetActive(true);
            // _gameEndPanel.ActiveGameEndPanel("Win!");]
            await StateMachine.GetInstance().ChangeState("gameEnd");
            return;
        }

        // if (_player.GetHP() <= 0)
        // {
        //     _gameEndPanel.gameObject.SetActive(true);
        //     _gameEndPanel.ActiveGameEndPanel("Lose!");
        //     return;
        // }

        Destroy(card.gameObject);
        await StateMachine.GetInstance().ChangeState("turnEnd");
    }
}