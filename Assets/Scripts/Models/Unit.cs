using System;
using Data;
namespace Models
{
    public class Unit
    {
        public event Action OnDied;
        
        public readonly UnitType Type;
        public readonly UnitAbility Ability;
        public readonly Abilities Abilities;
        public readonly bool IsEnemy;
        public UnitStates State;
        
        public UnitData UnitData;

        public Unit(UnitData unitData, bool isEnemy)
        {
            UnitData = unitData;
            Type = unitData.type;
            Ability = unitData.ability;
            Abilities = new Abilities();
            State = UnitStates.Inactive;
            IsEnemy = isEnemy;
            OnDied = null;
        }

        public void TakeDamage(int damage)
        {
            UnitData.health -= damage;
            if (UnitData.health <= 0)
            {
                UnitData.health = 0;
                Die();
            }
        }

        public void Heal(int amount)
        {
            UnitData.health += amount;
            if (UnitData.health > UnitData.maxHealth)
            {
                UnitData.health = UnitData.maxHealth;
            }
        }

        public void Buff(int buff) => 
            UnitData.maxHealth += buff;
        
        public void Debuff(int debuff) => 
            UnitData.maxHealth -= debuff;
        
        public void UseAbility(Unit target)
        {
            if (Ability == UnitAbility.Heal)
                Abilities.Heal(UnitData.abilityPower, target);
            else if (Ability == UnitAbility.DealDamage) 
                Abilities.DealDamage(UnitData.abilityPower, target);
        }

        private void Die() => 
            OnDied?.Invoke();
    }

}

