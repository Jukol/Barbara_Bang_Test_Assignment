namespace Models
{
    public struct Unit
    {
        public UnitType Type;
        public bool Enemy;
        
        public readonly UnitData UnitData;

        public Unit(UnitData unitData, UnitType unitType, bool enemy)
        {
            UnitData = unitData;
            Type = unitType;
            Enemy = enemy;
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

        public void Die()
        {
            // Do something
        }
    }

}

