using System.Collections.Generic;
using System.Linq;
using Models;
using UnityEngine;
namespace View
{
    public class BattleSystem : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        
        private List<UnitView> _friends;
        private List<UnitView> _enemies;
        
        private bool _anyAimingFriend;
        private bool _anyTargetedFriend;
        private bool _anyTargetedEnemy;
        
        private Vector2 _lineStart;
        private Vector2 _lineEnd;
        
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

        private void OnFriendClicked(UnitView unitView) => 
            ChooseUnitOrTarget(unitView);

        private void ChooseUnitOrTarget(UnitView unitView)
        {
            GetUnitStatesInfo();

            if ((_anyAimingFriend && _anyTargetedFriend) || _anyTargetedEnemy) return;

            if (unitView.Unit.State == UnitStates.Inactive)
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
        
        public void FriendsTurn()
        {
            var target = _enemies.FirstOrDefault(x => x.Unit.State == UnitStates.Targeted);
            var activeUnit = _friends.FirstOrDefault(x => x.Unit.State == UnitStates.Aiming);
            
            if (target != null && activeUnit != null)
            {
                activeUnit.Unit.UseAbility(target.Unit);
                target.HealthUpdate();
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
