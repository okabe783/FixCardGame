using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>Enemyの管理</summary>
public class Enemy : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private int _enemyDataList;
    private int _currentHp;
    private int _id;
    private string _effectName;
    private EnemyAttribute _attribute;
    private EnemySettings _enemyBase;

    #region ゲッターメソッド

    public EnemyAttribute GetAttribute() => _attribute;
    public string GetEffectName() => _effectName;
    public int GetCurrentHp() => _currentHp;
    public Image GetIcon() => _icon;

    #endregion
    

    public void SetCurrentHp(int hp)
    {
        _currentHp -= hp;
    }

    private void Awake()
    {
        SetEnemy(_enemyDataList);
    }
    
    private void Start()
    {
        _attribute = GetRandomAttribute();
    }

    private EnemyAttribute GetRandomAttribute()
    {
        var allAttributes = Enum.GetValues(typeof(EnemyAttribute));

        EnemyAttribute index = 0;
        int attributeCount = allAttributes.Length;
        
        index = (EnemyAttribute)allAttributes.GetValue(Random.Range(0, attributeCount));

        return index;
    }

    //Enemyの情報をセット
    private void SetEnemy(int enemyID)
    {
        EnemySettings enemyBase = Resources.Load<EnemySettings>("SOPrefabs/Enemy/Enemy" + enemyID);
        _icon.sprite = enemyBase.Sprite;
        _enemyBase = enemyBase;
        _currentHp = enemyBase.Hp;
        _id = enemyBase.EnemyID;
        _effectName = enemyBase.EffectName;
    }
}