using System;
using Data;
namespace Models
{
    [Serializable]
    public class UnitData
    {
        public UnitType type;
        public int health;
        public int maxHealth;
        public UnitAbility ability;
    }
}
