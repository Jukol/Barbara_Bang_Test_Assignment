using System;
using Data;
using UnityEngine;

namespace Models
{
    public class UnitCreator
    {
        private const string Tank = "Configs2/Tank";
        private const string DamageDealer = "Configs2/DamageDealer";
        private const string Healer = "Configs2/Healer";
        
        public Unit CreateUnit(UnitType type, bool iSenemy, int level)
        {
            switch (type)
            {
                case UnitType.Tank:
                    return new Unit(Deserialize(Tank).unitDataByLevels[level], iSenemy);
                case UnitType.DamageDealer:
                    return new Unit(Deserialize(DamageDealer).unitDataByLevels[level], iSenemy);
                case UnitType.Healer:
                    return new Unit(Deserialize(Healer).unitDataByLevels[level], iSenemy);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private UnitsData Deserialize(string jsonPath)
        {
            TextAsset file = Resources.Load<TextAsset>(jsonPath);
            return JsonUtility.FromJson<UnitsData>(file.text);
        }
    }
}
