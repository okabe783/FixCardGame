using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class InGameLogic : SingletonMonoBehaviour<InGameLogic>
{
    [SerializeField, Header("Cardを配る場所")] private PlayerHand _playerHand;
    [SerializeField, Header("置きたい場所")] private GameObject _targetTransform;
    [SerializeField, Header("全てのCardの数")] private int cardDataList;
    [SerializeField] private InGameView _inGameView;
    [SerializeField] private CardGenerator _cardGenerator;

    private Player _player;
    private Enemy _enemy;
    // Eventの発行
    private readonly ReactiveProperty<InGamePhase> _currentPhase = new();
    public IReactiveProperty<InGamePhase> CurrentPhase => _currentPhase;
    public PlayerHand PlayerHand => _playerHand;

    private void Start()
    {
        _enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();

        if (_enemy == null)
        {
            Debug.LogError("敵がみつかりません");
            return;
        }

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("CardGeneratorが見つかりません");
            return;
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
        List<int> cardIDs = Enumerable.Range(0, cardDataList).ToList();
        cardIDs = cardIDs.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < 3; i++)
        {
            Card createCard = _cardGenerator.SpawnCard(cardIDs[i]);
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
            await StateMachine.GetInstance().ChangeState("gameEnd");
            return;
        }

        Destroy(card.gameObject);
        await StateMachine.GetInstance().ChangeState("turnEnd");
    }
}