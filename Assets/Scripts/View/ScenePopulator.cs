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
        [SerializeField] private Transform friends;
        [SerializeField] private Transform enemies;
        
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
            List<UnitView> friendUnits = new();
            List<UnitView> enemyUnits = new();    
            
            foreach (var friend in _unitInitializer.Friends)
            {
                var unitView = Instantiate(unitPrefab, friends);
                unitView.Init(friend, Color.green);

                var unitTransform = unitView.transform;
                unitTransform.position = unitView.Unit.Type switch
                {
                    UnitType.Tank => friendTankPosition.position,
                    UnitType.Healer => friendHealerPosition.position,
                    UnitType.DamageDealer => friendDamageDealerPosition.position,
                    _ => unitTransform.position
                };

                friendUnits.Add(unitView);
            }
            
            foreach (var enemy in _unitInitializer.Enemies)
            {
                var unitView = Instantiate(unitPrefab, enemies);
                unitView.Init(enemy, Color.blue);
                
                var unitTransform = unitView.transform;
                unitTransform.position = unitView.Unit.Type switch
                {
                    UnitType.Tank => enemyTankPosition.position,
                    UnitType.Healer => enemyHealerPosition.position,
                    UnitType.DamageDealer => enemyDamageDealerPosition.position,
                    _ => unitTransform.position
                };
                
                enemyUnits.Add(unitView);
            }
            
            battleSystem.Init(friendUnits, enemyUnits);
        }
    }
}
