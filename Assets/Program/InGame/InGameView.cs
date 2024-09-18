using UnityEngine;

//InGameのロジックを管理する
public class InGameView : MonoBehaviour
{
    [SerializeField,Header("Cardを生成するクラス")] private CardGenerator _cardGenerator;
    [SerializeField, Header("Cardを配る場所")] private PlayerHand _playerHand;

    private void Start()
    {
        SetUp();
    }

    private void SetUp()
    {
        AddCardToHand();
    }

    //手札を配布する
    private void AddCardToHand()
    {
        for (int i = 0; i < 3; i++)
        {
            Card createCard = _cardGenerator.SpawnCard();
            //生成したカードを手札に加える
            _playerHand.AddCard(createCard);
        }
        
        _playerHand.ResetPosition();
    }
}
