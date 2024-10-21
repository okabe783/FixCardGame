using UnityEngine;
using DG.Tweening;
using UniRx;
using Cysharp.Threading.Tasks;

// InGameのロジックを管理する
public class InGameLogic : SingletonMonoBehaviour<InGameLogic>
{
    [SerializeField, Header("Cardを生成するクラス")]
    private CardGenerator _cardGenerator;

    [SerializeField, Header("Cardを配る場所")] private PlayerHand _playerHand;
    [SerializeField, Header("手札の位置")] private PlayerHand _homeTransform;
    [SerializeField, Header("置きたい場所")] private GameObject _targetTransform;

    [SerializeField] private GameObject _startPos;
    [SerializeField] private GameObject _targetPos;

    // Eventの発行
    private ReactiveProperty<InGamePhase> _currentPhase = new();

    public IReactiveProperty<InGamePhase> CurrentPhase => _currentPhase;

    public async UniTask PlayCard(Card card)
    {
        //ToDo:Panelをセットする
        //ToDo:Viewに通知してViewがこの処理を呼び出す
        // Cardをターゲットにセットする
        await card.transform.DOMove(_targetTransform.transform.position, 0.5f);
        _homeTransform.RemoveCard(card);
        await StateMachine.GetInstance().ChangeState("battle","battle");
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

    public async UniTask ActivePhasePanel(string panelName)
    {
        PhasePanel panelPrefab = Resources.Load<PhasePanel>("Panel/CurrentPhasePanel");

        if (panelPrefab == null)
        {
            Debug.LogError("Resourcesが存在しません");
            return;
        }

        PhasePanel panelInstance = Instantiate(panelPrefab, transform);
        panelInstance.UpdatePanelText(panelName);
        await UniTask.Delay(1000);
        Destroy(panelInstance.gameObject);
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
            EffectSettings effectPrefab = Resources.Load<EffectSettings>("Motion/" + card.GetEffectName());
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
            }
            EffectSettings effectInstance = Instantiate(effectPrefab,_startPos.transform.position,Quaternion.identity);
            await effectInstance.MoveEffectToTarget(effectInstance,_targetPos.transform.position);
            Destroy(effectInstance.gameObject);
        }
        
        await StateMachine.GetInstance().ChangeState("turnEnd","turnEnd");
        Destroy(card.gameObject);
    }
}