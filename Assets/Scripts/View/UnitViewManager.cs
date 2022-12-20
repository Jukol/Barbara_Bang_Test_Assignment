using System.Collections.Generic;
using Data;
using Infrastructure;
using Models;
using UnityEngine;
using System.Linq;

namespace View
{
    public class UnitViewManager : MonoBehaviour
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
        
        private readonly List<UnitView> _friendUnits = new();
        private readonly List<UnitView> _enemyUnits = new();

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
                
                unitView.Deselect();
                unitView.OnUnitClicked += OnUnitClicked;
                _friendUnits.Add(unitView);
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
                
                unitView.Deselect();
                unitView.OnUnitClicked += OnEnemyClicked;
                _enemyUnits.Add(unitView);
            }
        }
        
        private void OnUnitClicked(UnitView unitView)
        {
            var anyAiming = _friendUnits.Any(x => x.Unit.State == UnitStates.Aiming);
            var anyTargeted = _friendUnits.Any(x => x.Unit.State == UnitStates.Targeted);

            if (anyAiming && anyTargeted) return;
            
            if (unitView.Unit.State == UnitStates.Inactive)
            {
                if (anyAiming)
                {
                    unitView.Unit.State = UnitStates.Targeted;
                    unitView.Select();
                }
                else
                {
                    unitView.Unit.State = UnitStates.Aiming;
                    unitView.Select();
                }
            }

            foreach (var enemy in _enemyUnits)
            {
                enemy.Unit.State = UnitStates.Inactive;
                enemy.Deselect();
            }
            
            unitView.UnitViewUpdate();
        }

        private void OnEnemyClicked(UnitView unitView)
        {
            foreach (var enemy in _enemyUnits) enemy.Deselect();
            unitView.Select();
            unitView.Unit.State = UnitStates.Targeted;
            
            foreach (var friend in _friendUnits)
            {
                if (friend.Unit.State == UnitStates.Targeted)
                {
                    friend.Unit.State = UnitStates.Inactive;
                    friend.Deselect();
                }
            }
            
            unitView.UnitViewUpdate();
        }

        public void FriendsTurn()
        {
            var target = _enemyUnits.FirstOrDefault(x => x.Unit.State == UnitStates.Targeted);
            var activeUnit = _friendUnits.FirstOrDefault(x => x.Unit.State == UnitStates.Aiming);
            
            if (target != null && activeUnit != null)
            {
                activeUnit.Unit.UseAbility(target.Unit);
                target.UnitViewUpdate();
            }
        }

        public void ResetAll()
        {
            foreach (var friend in _friendUnits)
            {
                friend.Unit.State = UnitStates.Inactive;
                friend.Deselect();
                friend.UnitViewUpdate();
            }
            foreach (var enemy in _enemyUnits)
            {
                enemy.Unit.State = UnitStates.Inactive;
                enemy.Deselect();
                enemy.UnitViewUpdate();
            }
        }
    }
}
