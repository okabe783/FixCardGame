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

    // Eventの発行
    private ReactiveProperty<InGamePhase> _currentPhase = new();

    public IReactiveProperty<InGamePhase> CurrentPhase => _currentPhase;

    public async UniTask PlayCard(Card card)
    {
        // Cardをターゲットにセットする
        await card.transform.DOMove(_targetTransform.transform.position, 0.5f);
        _homeTransform.RemoveCard(card);
        ChangePhaseState(_currentPhase.Value);
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

    // Phaseを変更する
    public void ChangePhaseState(InGamePhase phase)
    {
        switch (phase)
        {
            case InGamePhase.Mulligan:
                CurrentPhase.Value = InGamePhase.TurnStart;
                break;
            case InGamePhase.TurnStart:
                CurrentPhase.Value = InGamePhase.Play;
                break;
            case InGamePhase.Play:
                CurrentPhase.Value = InGamePhase.Battle;
                break;
            case InGamePhase.Battle:
                CurrentPhase.Value = InGamePhase.TurnEnd;
                break;
            case InGamePhase.TurnEnd:
                CurrentPhase.Value = InGamePhase.TurnStart;
                break;
        }
    }

    public async UniTask ActivePhasePanel(string panelName, int delayTime)
    {
        PhasePanel panelPrefab = Resources.Load<PhasePanel>("Panel/CurrentPhasePanel");

        if (panelPrefab == null)
        {
            Debug.LogError("Resourcesが存在しません");
            return;
        }

        PhasePanel panelInstance = Instantiate(panelPrefab, transform);
        panelInstance.UpdatePanelText(panelName);
        await UniTask.Delay(delayTime);
        Destroy(panelInstance.gameObject);
    }

    //ToDo:Effectの座標をなんとかする
    private async UniTask CardBattle(Card card)
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
            Debug.Log("Damageが跳ね返された");
            var effectPrefab = Resources.Load<EffectSettings>("Motion/" + enemy.GetEffectName());
            if (effectPrefab == null)
            {
                Debug.LogError("Resourcesが読み込めません");
            }
            EffectSettings effectInstance = Instantiate(effectPrefab,enemy.transform.position,Quaternion.identity);
            Debug.Log(effectInstance.transform.position);
            await effectInstance.MoveEffectToTarget(effectInstance,card.transform.position);
            Destroy(effectInstance.gameObject);
        }

        Destroy(card.gameObject);
        ChangePhaseState(InGamePhase.Battle);
    }
}