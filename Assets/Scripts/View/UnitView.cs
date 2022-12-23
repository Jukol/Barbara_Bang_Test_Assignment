using System;
using Models;
using TMPro;
using UnityEngine;
namespace View
{
    public class UnitView : MonoBehaviour
    {
        public event Action<UnitView> OnUnitClicked; 
        public event Action<UnitView> OnUnitViewDied;
        public Unit Unit;

        [SerializeField] private int health;
        [SerializeField] private int maxHealth;
        [SerializeField] private SpriteRenderer selection;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text healthView;

        [SerializeField] private GameObject healthBar;

        [SerializeField] private SpriteRenderer spriteRenderer;

        private float _healthBarInitialScaleX;

        public void Init(Unit unit, Color color)
        {
            Unit = unit;
            spriteRenderer.color = color;
            
            health = unit.UnitData.health;
            maxHealth = unit.UnitData.maxHealth;
            
            Unit.OnDied += OnUnitDied;
            
            _healthBarInitialScaleX = healthBar.transform.localScale.x;
            
            titleText.text = unit.UnitData.type.ToString();
            healthView.text = health.ToString();
            
            UnitViewUpdate();
        }
        
        public void UnitViewUpdate()
        {

            if (Unit.State == UnitStates.Aiming)
                selection.color = Color.yellow;
            else if (Unit.State == UnitStates.Targeted) 
                selection.color = Color.red;
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

            healthView.text = health.ToString();
        }
        
        private void OnUnitDied()
        {
            Unit.OnDied -= OnUnitDied;
            Destroy(gameObject);
            OnUnitViewDied?.Invoke(this);
        }
    }
}
