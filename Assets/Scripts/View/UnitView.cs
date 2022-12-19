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
            
            float fullHealthView = healthBar.transform.localScale.x;
        }
    }
}
