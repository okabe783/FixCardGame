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

    [SerializeField]private bool isDebugMode;
    public EnemyAttribute GetAttribute()
    {
        return _attribute;
    }

    public string GetEffectName()
    {
        return _effectName; 
    }

    public int GetCurrentHp()
    {
        return _currentHp;
    }

    public void SetCurrentHp(int hp)
    {
        _currentHp -= hp;
    }

    private void Awake()
    {
        SetEnemy(1);
    }

    private void Start()
    {
        if (isDebugMode)
        {
            Debug.Log(_attribute);
        }
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
