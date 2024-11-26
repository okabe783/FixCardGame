using System;
using System.Linq;
using System.Collections.Generic;

// Cardの属性を管理する
public class CardAttribute
{
    private static List<EnemyAttribute> _allAttributes = new();
    
    public static List<EnemyAttribute> AllAttributes => _allAttributes;

    static CardAttribute()
    {
        SetActiveSkillList();
    }
    
    private static void SetActiveSkillList()
    {
        // 属性を配列に格納
        List<EnemyAttribute> enemyAttributes = Enum.GetValues(typeof(EnemyAttribute)).Cast<EnemyAttribute>().ToList();

        // 1属性の組合せ
        foreach (EnemyAttribute attributes in enemyAttributes)
        {
            _allAttributes.Add(attributes);
        }

        // 2属性の組合せ
        for (int i = 0; i < enemyAttributes.Count; i++)
        {
            for (int j = i + 1; j < enemyAttributes.Count; j++)
            {
                _allAttributes.Add(enemyAttributes[i] | enemyAttributes[j]);
            }
        }

        // 3属性の組合せ
        for (int i = 0; i < enemyAttributes.Count; i++)
        {
            for (int j = i + 1; j < enemyAttributes.Count; j++)
            {
                for (int k = j + 1; k < enemyAttributes.Count; k++)
                {
                    _allAttributes.Add(enemyAttributes[i] | enemyAttributes[j] | enemyAttributes[k]);
                }
            }
        }
    }
}