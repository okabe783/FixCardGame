using UnityEngine;

namespace Program.Enemy
{
    public class EnemyPowerUpSkill : IAbility
    {
        private IAbility _abilityImplementation;
        
        public string Name()　=> "攻撃力上昇スキル";

        public void SetAbility()
        {
            Debug.Log("敵のPowerをあげる処理");
        }
    }
}