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
        
        public void Initialize(int level)
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
            
            Friends.Add(_unitCreator.CreateUnit(UnitType.Tank, false, level));
            Friends.Add(_unitCreator.CreateUnit(UnitType.DamageDealer, false, level));
            Friends.Add(_unitCreator.CreateUnit(UnitType.Healer, false, level));
            
            Enemies.Add(_unitCreator.CreateUnit(UnitType.Tank, true, level));
            Enemies.Add(_unitCreator.CreateUnit(UnitType.DamageDealer, true, level));
            Enemies.Add(_unitCreator.CreateUnit(UnitType.Healer, true, level));
        }
    }
}
