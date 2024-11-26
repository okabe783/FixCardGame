using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>Enemyの管理</summary>
public class Enemy : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private int _enemyDataList;
    private int _currentHp;
    private int _power = 1;
    private int _powerValue;
    private int _healValue;

    private int _id;

    //　何ターン目にスキルを発動するかを設定
    private int _activeSkillTurn;
    private string _effectName;

    private EnemyAttribute _attribute;
    private EnemySettings _enemyBase;

    private SkillDatabase _skillDatabase;

    #region ゲッターメソッド

    public EnemyAttribute GetAttribute() => _attribute;
    public string GetEffectName() => _effectName;
    public int GetCurrentHp() => _currentHp;
    public int GetPower() => _power;
    public Image GetIcon() => _icon;

    #endregion

    public void SetCurrentHp(int hp) => _currentHp -= hp;

    public void SetPower() => _power += _powerValue;

    public EnemySettings GetEnemyBase() => _enemyBase;

    public int GetActiveSkillTurn() => _activeSkillTurn;

    public int GetHealValue() => _healValue;

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
        Array allAttributes = Enum.GetValues(typeof(EnemyAttribute));

        EnemyAttribute index = 0;
        int attributeCount = allAttributes.Length;

        index = (EnemyAttribute)allAttributes.GetValue(Random.Range(0, attributeCount));

        return index;
    }

    //Enemyの情報をセット
    private void SetEnemy(int enemyID)
    {
        var enemyIndex = Random.Range(1, enemyID);
        EnemySettings enemyBase = Resources.Load<EnemySettings>("SOPrefabs/Enemy/Enemy" + enemyIndex);
        _icon.sprite = enemyBase.Sprite;
        _enemyBase = enemyBase;
        _currentHp = enemyBase.Hp;
        _activeSkillTurn = enemyBase.ActiveSkillTurn;
        _id = enemyBase.EnemyID;
        _effectName = enemyBase.EffectName;
        _skillDatabase = enemyBase.CommandDatabase;

        // ToDo : スキルがついているときのみ入れる
        _powerValue = enemyBase.powerValue;
        _healValue = enemyBase.HealValue;
    }

    public async UniTask ActiveSkill(Enemy enemy)
    {
        if (_skillDatabase != null)
        {
            await _skillDatabase.ActiveSkill(enemy);
        }
    }
}