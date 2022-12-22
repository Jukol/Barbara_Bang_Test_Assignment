using System.Collections.Generic;
using Data;
using Infrastructure;
namespace Models
{
    public class UnitInitializer
    {
        private DependencyInjector _dependencyInjector;
        
        public UnitInitializer(DependencyInjector dependencyInjector) => 
            _dependencyInjector = dependencyInjector;

        private UnitCreator _unitCreator;

        public List<Unit> Friends;
        public List<Unit> Enemies;
        
        public void Initialize()
        {
            _unitCreator = _dependencyInjector.Resolve<UnitCreator>();

            if (Friends != null)
                Friends.Clear();
            else
                Friends = new List<Unit>();

            if (Enemies != null)
                Enemies.Clear();
            else
                Enemies = new List<Unit>();
            
            Friends.Add(_unitCreator.CreateUnit(UnitType.Tank, false));
            Friends.Add(_unitCreator.CreateUnit(UnitType.DamageDealer, false));
            Friends.Add(_unitCreator.CreateUnit(UnitType.Healer, false));
            
            Enemies.Add(_unitCreator.CreateUnit(UnitType.Tank, true));
            Enemies.Add(_unitCreator.CreateUnit(UnitType.DamageDealer, true));
            Enemies.Add(_unitCreator.CreateUnit(UnitType.Healer, true));
        }
    }
}
