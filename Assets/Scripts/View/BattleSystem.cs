using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Models;
using UnityEngine;
using Random = UnityEngine.Random;

namespace View
{
    public class BattleSystem : MonoBehaviour
    {
        private bool _gameOver;
        
        public event Action OnWin;
        public event Action OnLose;
        
        public bool NextTurn { get; private set; }
        
        [SerializeField] private LineRenderer lineRenderer;
        
        private List<UnitView> _friends;
        private List<UnitView> _enemies;
        
        private bool _anyAimingFriend;
        private bool _anyTargetedFriend;
        private bool _anyTargetedEnemy;
        
        private Vector2 _lineStart;
        private Vector2 _lineEnd;
        
        private readonly List<UnitView> _enemiesWithLoweredHealth = new();

#region Public Methods

        public void Init(List<UnitView> friends, List<UnitView> enemies)
        {
            _gameOver = false;
            
            _friends = friends;
            _enemies = enemies;

            foreach (var friend in _friends)
            {
                friend.Deselect();
                friend.OnUnitClicked += OnFriendClicked;
                friend.OnUnitViewDied += OnUnitViewDied;
            }

            foreach (var enemy in _enemies)
            {
                enemy.Deselect();
                enemy.OnUnitClicked += OnEnemyClicked;
                enemy.OnUnitViewDied += OnUnitViewDied;
            }
        }

        public async void FriendsTurn(Action onTurnEnd = null)
        {
            if (_gameOver) return;
            
            var targetEnemy = _enemies.FirstOrDefault(x => x.Unit.State == UnitStates.Targeted);
            var targetFriend = _friends.FirstOrDefault(x => x.Unit.State == UnitStates.Targeted);
            var aimingFriend = _friends.FirstOrDefault(x => x.Unit.State == UnitStates.Aiming);

            if (targetEnemy != null && aimingFriend != null)
                await DamageOrHeal(aimingFriend, targetEnemy);
            else if (targetFriend != null && aimingFriend != null)
                await DamageOrHeal(aimingFriend, targetFriend);
            else if (targetFriend != null && targetFriend.Unit.Type == UnitType.Healer && aimingFriend == null) 
                await DamageOrHeal(targetFriend, targetFriend); //self heal
            
            onTurnEnd?.Invoke();
        }

        public async void EnemiesTurn(Action onTurnEnd = null)
        {
            if (_gameOver) return;
            
            _enemiesWithLoweredHealth.Clear();

            for (var i = 0; i < _enemies.Count; i++)
            {
                var enemy = _enemies[i];
                enemy.Unit.State = UnitStates.Inactive;
                if (enemy.Unit.UnitData.health < enemy.Unit.UnitData.maxHealth) 
                    _enemiesWithLoweredHealth.Add(enemy);
            }

            var random = Random.Range(0, _enemies.Count);
            var aimingEnemy = _enemies[random];
            var typeOfAimingEnemy = aimingEnemy.Unit.Type;

            if (typeOfAimingEnemy == UnitType.Healer)
            {
                if (_enemiesWithLoweredHealth.Count == 0 && _enemies.Count > 1)
                {
                    EnemiesTurn();
                    return;
                }
                
                if (_enemies.Count == 1) //self heal
                {
                    ChangeStateAndUpdate(aimingEnemy, UnitStates.Targeted);
                    await DamageOrHeal(aimingEnemy, aimingEnemy);
                    onTurnEnd?.Invoke();
                    return;
                }
                
                var target = _enemiesWithLoweredHealth[Random.Range(0, _enemiesWithLoweredHealth.Count)];
                
                ChangeStateAndUpdate(aimingEnemy, UnitStates.Aiming);
                ChangeStateAndUpdate(target, UnitStates.Targeted);
                
                await DamageOrHeal(aimingEnemy, target);
            }
            else if (typeOfAimingEnemy is UnitType.Tank or UnitType.DamageDealer)
            {
                var target = _friends[Random.Range(0, _friends.Count)];
                
                ChangeStateAndUpdate(aimingEnemy, UnitStates.Aiming);
                ChangeStateAndUpdate(target, UnitStates.Targeted);
                
                await DamageOrHeal(aimingEnemy, target);
            }
            
            onTurnEnd?.Invoke();
            NextTurn = false;
        }
        
        public void ResetAll()
        {
            foreach (var friend in _friends)
            {
                friend.Unit.State = UnitStates.Inactive;
                friend.Deselect();
                friend.UnitViewUpdate();
            }
            foreach (var enemy in _enemies)
            {
                enemy.Unit.State = UnitStates.Inactive;
                enemy.Deselect();
                enemy.UnitViewUpdate();
            }
            lineRenderer.enabled = false;
        }

#endregion

#region Private Methods

        private void ChangeStateAndUpdate(UnitView unitView, UnitStates state)
        {
            unitView.Unit.State = state;
            unitView.Select();
            unitView.UnitViewUpdate();
        }
        
        private void OnFriendClicked(UnitView unitView)
        {
            ChooseSelfOrTarget(unitView);
            NextTurn = true;
        }

        private void OnEnemyClicked(UnitView unitView)
        {
            GetUnitStatesInfo();
            
            if (_anyTargetedEnemy || _anyTargetedFriend || !_anyAimingFriend) return;
            
            unitView.Select();
            unitView.Unit.State = UnitStates.Targeted;
            
            _lineEnd = unitView.transform.position;
            DrawLine(_lineStart, _lineEnd);

            unitView.UnitViewUpdate();
            NextTurn = true;
        }

        private void ChooseSelfOrTarget(UnitView unitView)
        {
            GetUnitStatesInfo();

            if ((_anyAimingFriend && _anyTargetedFriend) || _anyTargetedEnemy) return;

            if (unitView.Unit.State is UnitStates.Inactive or UnitStates.Aiming)
            {
                if (_anyAimingFriend)
                {
                    unitView.Unit.State = UnitStates.Targeted;
                        
                    _lineEnd = unitView.transform.position; 
                    DrawLine(_lineStart, _lineEnd);
                    
                    unitView.Select();
                }
                else
                {
                    unitView.Unit.State = UnitStates.Aiming;
                    _lineStart = unitView.transform.position;
                    
                    unitView.Select();
                }
            }

            foreach (var enemy in _enemies)
            {
                enemy.Unit.State = UnitStates.Inactive;
                enemy.Deselect();
            }

            unitView.UnitViewUpdate();
        }

        private void GetUnitStatesInfo()
        {
            _anyAimingFriend = _friends.Any(x => x.Unit.State == UnitStates.Aiming);
            _anyTargetedFriend = _friends.Any(x => x.Unit.State == UnitStates.Targeted);
            _anyTargetedEnemy = _enemies.Any(x => x.Unit.State == UnitStates.Targeted);
        }

        private void DrawLine(Vector3 start, Vector3 end)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
        }

        private async Task DamageOrHeal(UnitView aimingUnit, UnitView targetedUnit)
        {
            _lineStart = aimingUnit.transform.position;
            _lineEnd = targetedUnit.transform.position;
            DrawLine(_lineStart, _lineEnd);

            await Task.Delay(2000);

            aimingUnit.Unit.UseAbility(targetedUnit.Unit);
            
            aimingUnit.UnitViewUpdate();
            targetedUnit.UnitViewUpdate();
            targetedUnit.HealthUpdate();

            lineRenderer.enabled = false;

            foreach (var friend in _friends)
            {
                friend.Unit.State = UnitStates.Inactive;
                friend.Deselect();
            }

            foreach (var enemy in _enemies)
            {
                enemy.Unit.State = UnitStates.Inactive;
                enemy.Deselect();
            }
        }

        private void OnUnitViewDied(UnitView unitView)
        {
            if (!unitView.Unit.IsEnemy)
            {
                _friends.Remove(unitView);
                if (_friends.Count == 0)
                {
                    _gameOver = true;
                    OnLose?.Invoke();
                }
            }
            else
            {
                _enemies.Remove(unitView);
                if (_enemies.Count == 0)
                {
                    _gameOver = true;
                    OnWin?.Invoke();
                }
            }
        }

#endregion
        
    }
}
