using System;

namespace Models
{
    [Serializable]
    public class Abilities
    {
        public void DealDamage(int damage, Unit target) => 
            target.TakeDamage(damage);

        public void Heal(int heal, Unit target) => 
            target.Heal(heal);
    }
}
