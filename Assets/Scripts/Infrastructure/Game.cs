using Data;
using Models;
namespace Infrastructure
{
    public class Game
    {
        public DependencyInjector DependencyInjector;
        public int MaxLevels => 3;
        
        public void Initialize()
        {
            DependencyInjector = new DependencyInjector();
            DependencyInjector.Register(new Abilities());
            DependencyInjector.Register(new UnitCreator());
            DependencyInjector.Register(new UnitInitializer(DependencyInjector));
            
            DependencyInjector.Register(new UnitsData());
        }
    }
}