using System.Collections.Generic;
using Infrastructure;
namespace Models
{
    public class UnitInitializer
    {
        private DependencyInjector _dependencyInjector;
        
        public InitializeUnits(DependencyInjector dependencyInjector) => 
            _dependencyInjector = dependencyInjector;

        private UnitCreator _unitCreator;

        public List<Unit> Friends;
        public List<Unit> Enemies;
        
        public void Initialize()
        {
            _unitCreator = _dependencyInjector.Resolve<UnitCreator>();

            Friends = new List<Unit>()
            {
                _unitCreator.CreateUnit(UnitType.Tank, false),
                _unitCreator.CreateUnit(UnitType.DamageDealer, false),
                _unitCreator.CreateUnit(UnitType.Healer, false)
            };
            
            Enemies = new List<Unit>()
            {
                _unitCreator.CreateUnit(UnitType.Tank, true),
                _unitCreator.CreateUnit(UnitType.DamageDealer, true),
                _unitCreator.CreateUnit(UnitType.Healer, true)
            };
        }
    }
}
