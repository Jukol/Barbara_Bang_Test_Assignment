using System;
using Data;
namespace Models
{
    [Serializable]
    public struct UnitData
    {
        public UnitType type;
        public int health;
        public int maxHealth;
        public UnitAbility ability;
        public int abilityPower;
    }
}
