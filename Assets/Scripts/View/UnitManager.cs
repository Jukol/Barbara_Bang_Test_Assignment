using Infrastructure;
using Models;
using UnityEngine;
namespace View
{
    public class UnitManager : MonoBehaviour
    {
        [SerializeField] private UnitView unitPrefab;

        private DependencyInjector _dependencyInjector;
        private UnitInitializer _unitInitializer;

        public void Init(DependencyInjector dependencyInjector)
        {
            _dependencyInjector = dependencyInjector;
            _unitInitializer = _dependencyInjector.Resolve<UnitInitializer>();
            _unitInitializer.Initialize();
            
            PopulateScene();
        }

        private void PopulateScene()
        {
            foreach (var friend in _unitInitializer.Friends)
            {
                var unit = Instantiate(unitPrefab, transform);
                unit.Init(friend);
            }
        }
    }
}
