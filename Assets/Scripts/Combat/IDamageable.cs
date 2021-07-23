using UnityEngine;


namespace Disjointed.Combat
{
    public interface IDamageable
    {
        public void TakeDamage(float damage, string origin);
        public void TakeKnockback(Vector2 direction, float knockback);
    }
}