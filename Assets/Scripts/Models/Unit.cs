using System;
using Data;
namespace Models
{
    public class Unit
    {
        public event Action OnDied;
        
        public readonly UnitType Type;
        public readonly bool IsEnemy;
        public UnitStates State;
        public UnitData UnitData;
        
        private readonly UnitAbility _ability;
        private readonly Abilities _abilities;

        public Unit(UnitData unitData, bool isEnemy)
        {
            UnitData = unitData;
            Type = unitData.type;
            _ability = unitData.ability;
            _abilities = new Abilities();
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

        public void UseAbility(Unit target)
        {
            if (_ability == UnitAbility.Heal)
                _abilities.Heal(UnitData.abilityPower, target);
            else if (_ability == UnitAbility.DealDamage) 
                _abilities.DealDamage(UnitData.abilityPower, target);
        }

        private void Die() => 
            OnDied?.Invoke();
    }

}

