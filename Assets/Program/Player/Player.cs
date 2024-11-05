using UnityEngine;

/// <summary>Playerが持つパラメーターを管理</summary>
public class Player : MonoBehaviour
{
    [SerializeField] private int _hp = 10;

    public int GetHP() => _hp;
    
    public void ChangeHealth(int value)
    {
        _hp -= value;
    }
}