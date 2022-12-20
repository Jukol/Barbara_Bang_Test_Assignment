using System;
using Models;
using UnityEngine;
namespace View
{
    public class UnitView : MonoBehaviour
    {
        public event Action<UnitView> OnUnitClicked; 
        public Unit Unit;
        
        [SerializeField] private string unitType;
        [SerializeField] private int health;
        [SerializeField] private int maxHealth;
        [SerializeField] private string primaryAbility;
        [SerializeField] private SpriteRenderer selection;
        [SerializeField] private UnitStates stateView;

        [SerializeField] private GameObject healthBar;

        [SerializeField] private SpriteRenderer spriteRenderer;

        
        public void Init(Unit unit, Color color)
        {
            Unit = unit;
            spriteRenderer.color = color;
            
            unitType = unit.Type.ToString();
            health = unit.UnitData.health;
            maxHealth = unit.UnitData.maxHealth;
            primaryAbility = unit.UnitData.ability.ToString();
            stateView = unit.State;
            UnitViewUpdate();
        }
        
        public void UnitViewUpdate()
        {
            health = Unit.UnitData.health;
            maxHealth = Unit.UnitData.maxHealth;
            stateView = Unit.State;

            if (Unit.State == UnitStates.Aiming)
            {
                selection.color = Color.yellow;
            }
            else if (Unit.State == UnitStates.Targeted)
            {
                selection.color = Color.red;
            }
        }

        private void OnMouseDown() => 
            OnUnitClicked?.Invoke(this);

        public void Select()
        {
            selection.gameObject.SetActive(true);
        }

        public void Deselect()
        {
            selection.gameObject.SetActive(false);
            Unit.State = UnitStates.Inactive;
            stateView = Unit.State;
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
