using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField ]private int _hp = 10;

    public int GetHP()
    {
        return _hp;
    }

    public void ChangeHealth(int value)
    {
        _hp -= value;
    }
}
