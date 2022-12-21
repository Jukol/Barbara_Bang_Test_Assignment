using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Models;
using UnityEngine;
namespace View
{
    public class BattleSystem : MonoBehaviour
    {
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

        
        public void Init(List<UnitView> friends, List<UnitView> enemies)
        {
            _friends = friends;
            _enemies = enemies;

            foreach (var friend in _friends)
            {
                friend.Deselect();
                friend.OnUnitClicked += OnFriendClicked;
            }

            foreach (var enemy in _enemies)
            {
                enemy.Deselect();
                enemy.OnUnitClicked += OnEnemyClicked;
            }
        }

        private void OnFriendClicked(UnitView unitView)
        {
            ChooseUnitOrTarget(unitView);
            NextTurn = true;
        }

        private void ChooseUnitOrTarget(UnitView unitView)
        {
            GetUnitStatesInfo();

            if ((_anyAimingFriend && _anyTargetedFriend) || _anyTargetedEnemy) return;

            if (unitView.Unit.State is UnitStates.Inactive or UnitStates.Aiming);
            {
                if (_anyAimingFriend)
                {
                    unitView.Unit.State = UnitStates.Targeted;
                        
                    _lineEnd = unitView.transform.position; ;
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
        
        public async void FriendsTurn()
        {
            var targetEnemy = _enemies.FirstOrDefault(x => x.Unit.State == UnitStates.Targeted);
            var targetFriend = _friends.FirstOrDefault(x => x.Unit.State == UnitStates.Targeted);
            var aimingUnit = _friends.FirstOrDefault(x => x.Unit.State == UnitStates.Aiming);

            if (targetEnemy != null && aimingUnit != null)
            {
                await FireOrHeal(aimingUnit, targetEnemy);
            }
            else if (targetFriend != null && aimingUnit != null)
            {
                await FireOrHeal(aimingUnit, targetFriend);
            }
            else if (targetFriend != null && targetFriend.Unit.Type == UnitType.Healer && aimingUnit == null)
            {
                await FireOrHeal(targetFriend, targetFriend);//self heal
            }
        }

        public async void EnemiesTurn()
        {
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
                if (_enemiesWithLoweredHealth.Count == 0)
                {
                    EnemiesTurn();
                    return;
                }
                var target = _enemiesWithLoweredHealth[Random.Range(0, _enemiesWithLoweredHealth.Count)];
                aimingEnemy.Unit.State = UnitStates.Aiming;
                aimingEnemy.Select();
                aimingEnemy.UnitViewUpdate();
                target.Unit.State = UnitStates.Targeted;
                target.Select();
                target.UnitViewUpdate();
                
                await FireOrHeal(aimingEnemy, target);
            }
            else if (typeOfAimingEnemy is UnitType.Tank or UnitType.DamageDealer)
            {
                var target = _friends[Random.Range(0, _friends.Count)];
                aimingEnemy.Unit.State = UnitStates.Aiming;
                aimingEnemy.Select();
                aimingEnemy.UnitViewUpdate();
                target.Unit.State = UnitStates.Targeted;
                target.Select();
                target.UnitViewUpdate();
                
                await FireOrHeal(aimingEnemy, target);
            }

            Debug.Log("Enemy made move");
            
            NextTurn = false;
        }
        
        private async Task FireOrHeal(UnitView aimingUnit, UnitView targetedUnit)
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
    }
}
