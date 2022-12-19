using System;
using Data;
using UnityEngine;

namespace Models
{
    public class UnitCreator
    {
        private const string Tank = "Configs/Tank";
        private const string DamageDealer = "Configs/DamageDealer";
        private const string Healer = "Configs/Healer";
        
        public Unit CreateUnit(UnitType type, bool iSenemy)
        {
            switch (type)
            {
                case UnitType.Tank:
                    return new Unit(Deserialize(Tank), iSenemy);
                case UnitType.DamageDealer:
                    return new Unit(Deserialize(DamageDealer), iSenemy);
                case UnitType.Healer:
                    return new Unit(Deserialize(Healer), iSenemy);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private UnitData Deserialize(string jsonPath)
        {
            TextAsset file = Resources.Load<TextAsset>(jsonPath);
            var unitData = JsonUtility.FromJson<UnitData>(file.text);

            var type = unitData.type;
            var health = unitData.health;
            var maxHealth = unitData.maxHealth;
            var ability = unitData.ability;

            return unitData;
        }
    }
}
