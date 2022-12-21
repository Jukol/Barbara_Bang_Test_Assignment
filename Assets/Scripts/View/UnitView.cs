using System;
using Models;
using TMPro;
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
        [SerializeField] private TMP_Text titleText;
        

        [SerializeField] private GameObject healthBar;

        [SerializeField] private SpriteRenderer spriteRenderer;

        private float _healthBarInitialScaleX;

        
        public void Init(Unit unit, Color color)
        {
            Unit = unit;
            spriteRenderer.color = color;
            
            unitType = unit.Type.ToString();
            health = unit.UnitData.health;
            maxHealth = unit.UnitData.maxHealth;
            primaryAbility = unit.UnitData.ability.ToString();
            stateView = unit.State;
            
            _healthBarInitialScaleX = healthBar.transform.localScale.x;
            
            titleText.text = unit.UnitData.type.ToString();
            
            UnitViewUpdate();
        }
        
        public void UnitViewUpdate()
        {
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

        public void HealthUpdate()
        {
            health = Unit.UnitData.health;
            maxHealth = Unit.UnitData.maxHealth;

            float healthPercent = (float) health / maxHealth;
            float currentHealthView = _healthBarInitialScaleX * healthPercent;
            
            var localScale = healthBar.transform.localScale;
            var hbY = localScale.y;
            var hbZ = localScale.z;
            localScale = new Vector3(currentHealthView, hbY, hbZ);
            
            healthBar.transform.localScale = localScale;
        }
    }
}
