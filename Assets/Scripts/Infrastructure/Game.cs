using Models;
namespace Infrastructure
{
    public class Game
    {
        public DependencyInjector DependencyInjector;
        
        public void Initialize()
        {
            DependencyInjector = new DependencyInjector();
            DependencyInjector.Register(new Abilities(DependencyInjector));
            DependencyInjector.Register(new UnitCreator(DependencyInjector));
            DependencyInjector.Register(new UnitInitializer(DependencyInjector));
        }
    }
}