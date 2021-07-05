namespace OGAM.Combat
{
    public interface IDamageable
    {
        public void TakeDamage(float damage, float knockback, string origin);
    }
}