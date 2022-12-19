namespace Models
{
    public struct Unit
    {
        public UnitType Type;
        public bool Enemy;
        
        private readonly UnitData _unitData;

        public Unit(UnitData unitData, UnitType unitType, bool enemy)
        {
            _unitData = unitData;
            Type = unitType;
            Enemy = enemy;
        }

        public void TakeDamage(int damage)
        {
            _unitData.health -= damage;
            if (_unitData.health <= 0)
            {
                _unitData.health = 0;
                Die();
            }
        }

        public void Heal(int amount)
        {
            _unitData.health += amount;
            if (_unitData.health > _unitData.maxHealth)
            {
                _unitData.health = _unitData.maxHealth;
            }
        }

        public void Buff(int buff) => 
            _unitData.maxHealth += buff;
        
        public void Debuff(int debuff) => 
            _unitData.maxHealth -= debuff;

        public void Die()
        {
            // Do something
        }
    }

}

