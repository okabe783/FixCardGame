using UniRx;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

// InGameのロジックを管理する
public class InGameLogic : SingletonMonoBehaviour<InGameLogic>
{
    [SerializeField] private Player _player;
    [SerializeField, Header("Cardを生成するクラス")] private CardGenerator _cardGenerator;
    [SerializeField, Header("Cardを配る場所")] private PlayerHand _playerHand;
    [SerializeField, Header("手札の位置")] private PlayerHand _homeTransform;
    [SerializeField, Header("置きたい場所")] private GameObject _targetTransform;
    [SerializeField] private InGameView _inGameView;

    // Eventの発行
    private readonly ReactiveProperty<InGamePhase> _currentPhase = new();

    public IReactiveProperty<InGamePhase> CurrentPhase => _currentPhase;

    public PlayerHand PlayerHand => _playerHand;

    public async UniTask PlayCard(Card card)
    {
        card.GetIcon().enabled = false;
        // Cardをターゲットにセットする
        await card.transform.DOMove(_targetTransform.transform.position, 0.1f);
        await _inGameView.ShowEffect(card.GetSummonEffectName(),_targetTransform.transform.position);
        card.GetIcon().enabled = true;
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
        Enemy enemy = FindAnyObjectByType<Enemy>();

        if (enemy == null)
        {
            Debug.LogError("敵がみつかりません");
            return;
        }

        EnemyAttribute enemyAttribute = enemy.GetAttribute();
        EnemyAttribute cardAttribute = card.GetCardSkill();

        if ((enemyAttribute & cardAttribute) != 0)
        {
            enemy.SetCurrentHp(card.GetCardPower());
            _inGameView.ChangeHPBar(enemy.GetCurrentHp(), 0);
            //Effectの設定
            EffectSettings effectPrefab = Resources.Load<EffectSettings>("Motion/" + card.GetAttackEffectName());
            EffectSettings effectInstance = Instantiate(effectPrefab,card.transform.position,Quaternion.identity);
            await effectInstance.MoveEffectToTarget(effectInstance,enemy.transform.position);
            var hitEffectPrefab = Resources.Load<EffectSettings>("Motion/" + effectPrefab.OnHitEffect);
            EffectSettings hitEffectInstance = Instantiate(hitEffectPrefab,enemy.transform.position,Quaternion.identity);
            await hitEffectInstance.SetParticle(hitEffectInstance);
        }
        else
        {
            var effectPrefab = Resources.Load<EffectSettings>("Motion/" + enemy.GetEffectName());
            if (effectPrefab == null)
            {
                Debug.LogError("Resourcesが読み込めません");
                return;
            }
            
            //PlayerのHPを減らす
            _player.ChangeHealth(1);
            _inGameView.ChangeHPBar(_player.GetHP(), 1);
            EffectSettings effectInstance = Instantiate(effectPrefab,enemy.transform.position,Quaternion.identity);
            await effectInstance.MoveEffectToTarget(effectInstance,card.transform.position);
            var hitEffectPrefab = Resources.Load<EffectSettings>("Motion/" + effectPrefab.OnHitEffect);
            var hitEffectInstance = Instantiate(hitEffectPrefab,card.transform.position,Quaternion.identity);
            await hitEffectInstance.SetParticle(hitEffectInstance);
        }
        
        await StateMachine.GetInstance().ChangeState("turnEnd");
        Destroy(card.gameObject);
    }
}