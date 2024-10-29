using TMPro;
using UnityEngine;

public class GameEndPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _gameEndText;

    public void ActiveGameEndPanel(string gameEndText)
    {
        gameObject.SetActive(true);
        _gameEndText.text = gameEndText;
    }
}
