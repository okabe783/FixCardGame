using TMPro;
using UnityEngine;

public class HPSettings : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerHPText;
    [SerializeField] private TextMeshProUGUI _enemyHPText;

    private int _playerHP;
    private int _enemyHP;

    public void  UpdatePlayerHPText(int hp)
    {
        _playerHPText.text = $"自分の残りHP　:　{hp.ToString()}";
    }
    
    public void  UpdateEnemyHPText(int hp)
    {
        _enemyHPText.text = $"自分の残りHPは　:　{hp.ToString()}";
    }

    public int GetPlayerHP()
    {
        return _playerHP;
    }
    
    public int GetEnemyHPText()
    {
        return _enemyHP;
    }
}