using System;
using UnityEngine;

namespace Models
{
    public class UnitCreator
    {
        private const string Tank = "Configs/Tank";
        private const string DamageDealer = "Configs/DamageDealer";
        private const string Healer = "Configs/Healer";
        
        public Unit CreateUnit(UnitType type, bool enemy)
        {
            switch (type)
            {
                case UnitType.Tank:
                    return new Unit(Deserialize(Tank), UnitType.Tank, enemy);
                case UnitType.DamageDealer:
                    return new Unit(Deserialize(DamageDealer), UnitType.DamageDealer, enemy);
                case UnitType.Healer:
                    return new Unit(Deserialize(Healer), UnitType.Healer, enemy);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private UnitData Deserialize(string jsonPath)
        {
            TextAsset file = Resources.Load<TextAsset>(jsonPath);
            return JsonUtility.FromJson<UnitData>(file.text);
        }
    }
}
