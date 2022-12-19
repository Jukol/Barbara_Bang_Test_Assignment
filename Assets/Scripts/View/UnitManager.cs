using Infrastructure;
using Models;
using UnityEngine;
namespace View
{
    public class UnitManager : MonoBehaviour
    {
        [SerializeField] private UnitView unitPrefab;
        [SerializeField] private Transform friends;
        [SerializeField] private Transform enemies;
        
        [SerializeField] private Transform friendTankPosition;
        [SerializeField] private Transform friendDamageDealerPosition;
        [SerializeField] private Transform friendHealerPosition;
        
        [SerializeField] private Transform enemyTankPosition;
        [SerializeField] private Transform enemyDamageDealerPosition;
        [SerializeField] private Transform enemyHealerPosition;

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
                var unit = Instantiate(unitPrefab, friends);
                unit.Init(friend, Color.green);

                var unitTransform = unit.transform;
                unitTransform.position = unit.Unit.Type switch
                {
                    UnitType.Tank => friendTankPosition.position,
                    UnitType.Healer => friendHealerPosition.position,
                    UnitType.DamageDealer => friendDamageDealerPosition.position,
                    _ => unitTransform.position
                };
            }
            
            foreach (var enemy in _unitInitializer.Enemies)
            {
                var unit = Instantiate(unitPrefab, enemies);
                unit.Init(enemy, Color.red);
                
                var unitTransform = unit.transform;
                unitTransform.position = unit.Unit.Type switch
                {
                    UnitType.Tank => enemyTankPosition.position,
                    UnitType.Healer => enemyHealerPosition.position,
                    UnitType.DamageDealer => enemyDamageDealerPosition.position,
                    _ => unitTransform.position
                };
            }
        }
    }
}
