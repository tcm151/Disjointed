using UnityEngine;


namespace Disjointed.Combat
{
    public interface IDamageable
    {
        public void TakeDamage(float damage, string origin);
        public void TakeKnockback(float knockback, Vector2 direction);
    }
}