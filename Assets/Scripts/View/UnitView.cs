using System;
using Models;
using UnityEngine;
namespace View
{
    public class UnitView : MonoBehaviour
    {
        public event Action<UnitView> OnUnitClicked; 

        [SerializeField] private string unitType;
        [SerializeField] private string enemyOrFriend;
        [SerializeField] private int health;
        [SerializeField] private int maxHealth;
        [SerializeField] private string primaryAbility;
        [SerializeField] private GameObject selection;
        

        [SerializeField] private GameObject healthBar;

        [SerializeField] private SpriteRenderer spriteRenderer;

        public Unit Unit => _unit;
        private Unit _unit;
        
        public void Init(Unit unit, Color color)
        {
            _unit = unit;
            spriteRenderer.color = color;
            
            unitType = unit.Type.ToString();
            enemyOrFriend = unit.IsEnemy ? "Enemy" : "Friend";
            health = unit.UnitData.health;
            maxHealth = unit.UnitData.maxHealth;
            primaryAbility = unit.UnitData.ability.ToString();
        }

        private void OnMouseDown()
        {
            OnUnitClicked?.Invoke(this);
            Debug.Log("Clicked");
        }

        public void Select()
        {
            selection.SetActive(true);
            _unit.IsActive = true;
        }

        public void Deselect()
        {
            selection.SetActive(false);
            _unit.IsActive = false;
        }

        private void HandleHealthBar(Unit unit)
        {
            var localScale = healthBar.transform.localScale;
            float fullHealthView = localScale.x;
            float healthPercent = unit.UnitData.health / unit.UnitData.maxHealth;
            float currentHealthView = fullHealthView * healthPercent;
            localScale = new Vector3(currentHealthView, localScale.y, localScale.z);
            healthBar.transform.localScale = localScale;
        }
        
        
    }
}
