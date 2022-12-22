using System;
using System.Collections.Generic;
using Data;
using Infrastructure;
using Models;
using UnityEngine;

namespace View
{
    public class ScenePopulator : MonoBehaviour
    {
        [SerializeField] private UnitView unitPrefab;
        [SerializeField] private Transform friendUnits;
        [SerializeField] private Transform enemyUnits;
        
        [SerializeField] private Transform friendTankPosition;
        [SerializeField] private Transform friendDamageDealerPosition;
        [SerializeField] private Transform friendHealerPosition;
        
        [SerializeField] private Transform enemyTankPosition;
        [SerializeField] private Transform enemyDamageDealerPosition;
        [SerializeField] private Transform enemyHealerPosition;

        [SerializeField] private BattleSystem battleSystem;

        private DependencyInjector _dependencyInjector;
        private UnitInitializer _unitInitializer;
        
        private List<UnitView> _friendUnits = new();
        private List<UnitView> _enemyUnits = new();    


        public void Init(DependencyInjector dependencyInjector)
        {
            _dependencyInjector = dependencyInjector;
            _unitInitializer = _dependencyInjector.Resolve<UnitInitializer>();
            _unitInitializer.Initialize();
            
            PopulateScene();
        }

        private void PopulateScene()
        {
            ClearScene();
            
            foreach (var friend in _unitInitializer.Friends)
            {
                var unitView = Instantiate(unitPrefab, friendUnits);
                unitView.Init(friend, Color.green);

                var unitTransform = unitView.transform;
                unitTransform.position = unitView.Unit.Type switch
                {
                    UnitType.Tank => friendTankPosition.position,
                    UnitType.Healer => friendHealerPosition.position,
                    UnitType.DamageDealer => friendDamageDealerPosition.position,
                    _ => unitTransform.position
                };

                _friendUnits.Add(unitView);
            }
            
            foreach (var enemy in _unitInitializer.Enemies)
            {
                var unitView = Instantiate(unitPrefab, enemyUnits);
                unitView.Init(enemy, Color.blue);
                
                var unitTransform = unitView.transform;
                unitTransform.position = unitView.Unit.Type switch
                {
                    UnitType.Tank => enemyTankPosition.position,
                    UnitType.Healer => enemyHealerPosition.position,
                    UnitType.DamageDealer => enemyDamageDealerPosition.position,
                    _ => unitTransform.position
                };
                
                _enemyUnits.Add(unitView);
            }
            
            battleSystem.Init(_friendUnits, _enemyUnits);
        }

        private void ClearScene()
        {
            foreach (var friend in _friendUnits) Destroy(friend.gameObject);
            foreach (var enemy in _enemyUnits) Destroy(enemy.gameObject);

            _friendUnits.Clear();
            _enemyUnits.Clear();
        }
    }
}
