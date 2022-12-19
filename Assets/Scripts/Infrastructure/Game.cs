using Models;
namespace Infrastructure
{
    public class Game
    {
        public DependencyInjector DependencyInjector;
        
        public void Initialize()
        {
            DependencyInjector = new DependencyInjector();
            DependencyInjector.Register(new Abilities());
            DependencyInjector.Register(new UnitCreator());
            DependencyInjector.Register(new UnitInitializer(DependencyInjector));
        }
    }
}