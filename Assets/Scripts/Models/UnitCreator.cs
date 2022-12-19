using System;
using System.IO;
using Infrastructure;
using Newtonsoft.Json;

namespace Models
{
    public class UnitCreator
    {
        private DependencyInjector _dependencyInjector;
        
        public UnitCreator(DependencyInjector dependencyInjector) => 
            _dependencyInjector = dependencyInjector;
        
        private const string Tank = "Scripts/Configs/Tank.json";
        private const string DamageDealer = "Scripts/Configs/DamageDealer.json";
        private const string Healer = "Scripts/Configs/Healer.json";
        
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

        private UnitData Deserialize(string json)
        {
            string tankJson = File.ReadAllText(json);
            return JsonConvert.DeserializeObject<UnitData>(tankJson);
        }
    }
}
