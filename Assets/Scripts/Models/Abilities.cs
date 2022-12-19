using System;
using Infrastructure;

namespace Models
{
    [Serializable]
    public class Abilities
    {
        private DependencyInjector _dependencyInjector;
        
        public Abilities(DependencyInjector dependencyInjector) => 
            _dependencyInjector = dependencyInjector;

        public void DealDamage(int damage, Unit target) => 
            target.TakeDamage(damage);

        public void Heal(int heal, Unit target) => 
            target.Heal(heal);

        public void Buff(int buff, Unit target) => 
            target.Buff(buff);

        public void Debuff(int debuff, Unit target) => 
            target.Debuff(debuff);
    }
}
