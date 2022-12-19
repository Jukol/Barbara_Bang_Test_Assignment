using System;
namespace Models
{
    [Serializable]
    public class UnitData
    {
        public UnitType type;
        public int health;
        public int maxHealth;
        public float initCoordX;
        public float initCoordY;

        public Abilities primaryAbility;
        public Abilities secondaryAbility;
        public Abilities ultimateAbility;
    }
}
