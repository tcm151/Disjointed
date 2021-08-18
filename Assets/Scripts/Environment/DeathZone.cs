using UnityEngine;
using Disjointed.Combat;


namespace Disjointed.Environment
{
    public class DeathZone : MonoBehaviour
    {
        public LayerMask playerMask;
        
        private void OnTriggerEnter2D(Collider2D collider)
        {
            var damageable = collider.GetComponent<IDamageable>();
            if (damageable is null) return;
            
            damageable.TakeDamage(9001f, "Death Zone!");
        }
        
    }
}
