using System;

namespace Data
{
    [Serializable]
    public struct UnitsData
    {
        public UnitData[] unitDataByLevels;
    }

    [Serializable]
    public struct UnitData
    {
        public string name;
        public int level;
        public UnitType type;
        public int health;
        public int maxHealth;
        public UnitAbility ability;
        public int abilityPower;
    }
}
