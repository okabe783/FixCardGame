using Cysharp.Threading.Tasks;

public class BattlePhase : IState
{
    private Card _currentCard;

    public void SetCard(Card card)
    {
        _currentCard = card;
    }
    
    public async UniTask Enter()
    {
        await InGameSystem.I.PlayCard(_currentCard);
        await InGameSystem.I.CardBattle(_currentCard);
    }

    public async void Exit()
    {
        // 敵のスキルを発動する
        await InGameSystem.I.ActiveEnemySkill();
    }
}