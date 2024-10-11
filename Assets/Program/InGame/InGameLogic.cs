using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;

//InGameのロジックを管理する
public class InGameLogic : SingletonMonoBehaviour<InGameLogic>
{
    [SerializeField,Header("Cardを生成するクラス")] private CardGenerator _cardGenerator;
    [SerializeField, Header("Cardを配る場所")] private PlayerHand _playerHand;
    [SerializeField,Header("手札の位置")] private PlayerHand _homeTransform;
    [SerializeField,Header("置きたい場所")] private GameObject _targetTransform;
    
    //Eventの発行
    private ReactiveProperty<InGamePhase> _currentPhase = new();

    public IReactiveProperty<InGamePhase> CurrentPhase => _currentPhase;

    public void PlayCard(Card card)
    {
        //Cardをターゲットにセットする
        _homeTransform.RemoveCard(card);
        card.transform.position = _targetTransform.transform.position;
        //ToDo:Phaseで値を管理
        ChangePhaseState(_currentPhase.Value);
    }
    
    //手札を配布する
    public async UniTask AddCardToHand()
    {
        for (int i = 0; i < 3; i++)
        {
            Card createCard = _cardGenerator.SpawnCard();
            await createCard.transform.DOMove(_playerHand.transform.position, 0.5f);
            //生成したカードを手札に加える
            await _playerHand.AddCard(createCard);
        }
    }

    //Phaseを変更する
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
                CurrentPhase.Value = InGamePhase.Wait;
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
}