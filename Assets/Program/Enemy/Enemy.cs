using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Image _icon;
    private EnemySettings _enemyBase;
    private int _currentHp;
    private string _effectName;
    private int _id;
    private EnemyAttribute _attribute;

    public EnemyAttribute GetAttribute()
    {
        return _attribute;
    }

    public string GetEffectName()
    {
        return _effectName; 
    }

    public void SetCurrentHp(int hp)
    {
        _currentHp = hp;
        Debug.Log(_currentHp);
    }

    private void Start()
    {
        SetEnemy(1);
    }

    //Enemyの情報をセット
    public void SetEnemy(int enemyID)
    {
        EnemySettings enemyBase = Resources.Load<EnemySettings>("SOPrefabs/Enemy/Enemy" + enemyID);
        _icon.sprite = enemyBase.Sprite;
        _enemyBase = enemyBase;
        _currentHp = enemyBase.Hp;
        _id = enemyBase.EnemyID;
        _effectName = enemyBase.EffectName;
        _attribute = enemyBase.EnemyAttribute;
    }
}
