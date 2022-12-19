using Models;
using UnityEngine;
namespace View
{
    public class UnitView : MonoBehaviour
    {
        [SerializeField] private string unitType;
        [SerializeField] private string enemyOrFriend;
        [SerializeField] private GameObject healthBar;

        [SerializeField] private SpriteRenderer spriteRenderer;

        public Unit Unit => _unit;
        private Unit _unit;
        
        public void Init(Unit unit, Color color)
        {
            _unit = unit;
            spriteRenderer.color = color;
            
            unitType = unit.Type.ToString();
            enemyOrFriend = unit.Enemy ? "Enemy" : "Friend";

            HandleHealthBar(unit);
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
