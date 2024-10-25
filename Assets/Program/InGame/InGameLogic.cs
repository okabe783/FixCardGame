using UniRx;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

// InGameのロジックを管理する
public class InGameLogic : SingletonMonoBehaviour<InGameLogic>
{
    [SerializeField, Header("Cardを生成するクラス")] private CardGenerator _cardGenerator;
    [SerializeField, Header("Cardを配る場所")] private PlayerHand _playerHand;
    [SerializeField, Header("手札の位置")] private PlayerHand _homeTransform;
    [SerializeField, Header("置きたい場所")] private GameObject _targetTransform;
    [SerializeField] private GameObject _startPos;
    [SerializeField] private GameObject _targetPos;
    [SerializeField] private InGameView _inGameView;

    // Eventの発行
    private ReactiveProperty<InGamePhase> _currentPhase = new();

    public IReactiveProperty<InGamePhase> CurrentPhase => _currentPhase;

    public async UniTask PlayCard(Card card)
    {
        card.GetIcon().enabled = false;
        // Cardをターゲットにセットする
        await card.transform.DOMove(_targetTransform.transform.position, 0.1f);
        await _inGameView.ShowEffect(card.GetSummonEffectName(),_startPos.transform.position);
        card.GetIcon().enabled = true;
        _homeTransform.RemoveCard(card);
        await StateMachine.GetInstance().ChangeState("battle");
        await CardBattle(card);
    }

    // 手札を配布する
    public async UniTask AddCardToHand()
    {
        for (int i = 0; i < 3; i++)
        {
            Card createCard = _cardGenerator.SpawnCard();
            // 生成したカードを手札に加える
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
            EffectSettings effectPrefab = Resources.Load<EffectSettings>("Motion/" + card.GetAttackEffectName());
            //Canseltokenの使用
            EffectSettings effectInstance = Instantiate(effectPrefab,card.transform.position,Quaternion.identity);
            await effectInstance.MoveEffectToTarget(effectInstance,enemy.transform.position);
            Destroy(effectInstance.gameObject);
        }
        else
        {
            var effectPrefab = Resources.Load<EffectSettings>("Motion/" + enemy.GetEffectName());
            if (effectPrefab == null)
            {
                Debug.LogError("Resourcesが読み込めません");
                return;
            }
            EffectSettings effectInstance = Instantiate(effectPrefab,enemy.transform.position,Quaternion.identity);
            await effectInstance.MoveEffectToTarget(effectInstance,card.transform.position);
        }
        
        await StateMachine.GetInstance().ChangeState("turnEnd");
        Destroy(card.gameObject);
    }
}