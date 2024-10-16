using UnityEngine;
using DG.Tweening;
using UniRx;
using Cysharp.Threading.Tasks;

// InGameのロジックを管理する
public class InGameLogic : SingletonMonoBehaviour<InGameLogic>
{
    [SerializeField,Header("Cardを生成するクラス")] private CardGenerator _cardGenerator;
    [SerializeField, Header("Cardを配る場所")] private PlayerHand _playerHand;
    [SerializeField,Header("手札の位置")] private PlayerHand _homeTransform;
    [SerializeField,Header("置きたい場所")] private GameObject _targetTransform;
    
    // Eventの発行
    private ReactiveProperty<InGamePhase> _currentPhase = new();

    public IReactiveProperty<InGamePhase> CurrentPhase => _currentPhase;

    public void PlayCard(Card card)
    {
        // Cardをターゲットにセットする
        _homeTransform.RemoveCard(card);
        card.transform.DOMove(_targetTransform.transform.position, 0.2f);
        ChangePhaseState(_currentPhase.Value);
        CardBattle(card);
        Debug.Log(_currentPhase.Value);
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
            case InGamePhase.TurnEnd:
                CurrentPhase.Value = InGamePhase.TurnStart;
                break;
        }
    }
    
    public async UniTask ActivePhasePanel(string panelName,int delayTime)
    {
        PhasePanel panelPrefab = Resources.Load<PhasePanel>("Panel/CurrentPhasePanel");
        
        if (panelPrefab == null)
        {
            Debug.LogError("Resourcesが存在しません");
            return;
        }
        
        PhasePanel panelInstance = Instantiate(panelPrefab,transform);
        panelInstance.UpdatePanelText(panelName);
        await UniTask.Delay(delayTime);
        Destroy(panelInstance.gameObject);
    }

    public void CardBattle(Card card)
    {
        // Battleの処理
        // 出したカードが持つスキルで相手の属性を見分けていればカードのもつ攻撃力分のダメージを与える
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
            Debug.Log("DamageがHitした");
        }
        else
        {
            Debug.Log("Damageが跳ね返された");
        }
        
        //Cardをdestroyする処理
    }
}