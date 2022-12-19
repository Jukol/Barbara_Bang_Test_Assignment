using Models;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Infrastructure
{
    public class Bootstrapper : MonoBehaviour
    {
        private DependencyInjector _dependencyInjector;
        private void Awake()
        {
            _dependencyInjector = new DependencyInjector();
            _dependencyInjector.Register(new Abilities(_dependencyInjector));
            _dependencyInjector.Register(new UnitCreator(_dependencyInjector));
            _dependencyInjector.Register(new UnitInitializer(_dependencyInjector));
        }
        
        private void Start()
        {
            _dependencyInjector.Resolve<UnitInitializer>().Initialize();
            SceneManager.LoadScene(1);
        }
    }
}
