using System;
using Disjointed.Combat;
using Disjointed.Tools;
using UnityEngine;


namespace Disjointed.Player
{
    public class Combat : MonoBehaviour
    {
        public float damage = 1f;
        public float knockback = 10f;
        public Vector2 attackSize;
        
        private Vector3 lastAttackDirection;
        private Vector3 attackDirection;
        private float attackAngle;
        private bool attacking;

        //> HANDLE INPUT
        private void Update()
        {
            attacking |= Input.GetMouseButtonDown(0);
            
            attackDirection.x = Input.GetAxisRaw("Horizontal");
            attackDirection.y = Input.GetAxisRaw("Vertical");
            attackDirection.Normalize();

            if (attackDirection == Vector3.zero) attackDirection = lastAttackDirection;
            else lastAttackDirection = attackDirection;

            attackAngle = (attackDirection == Vector3.zero) ? attackAngle : attackDirection.Angle();
        }

        //> ATTACK
        private void FixedUpdate()
        {
            // ignore if not attacking
            if (!attacking) return;

            // cycle all overlap colliders
            var colliders = Physics2D.OverlapBoxAll(transform.position + (attackDirection * attackSize.x/2f), attackSize, attackAngle);
            foreach (var collider in colliders)
            {
                var damageable = collider.GetComponent<IDamageable>();
                if (damageable is null) continue;
                
                damageable.TakeDamage(damage, "Player Melee");

                var direction = (collider.transform.position - transform.position).normalized;
                damageable.TakeKnockback(knockback, direction);
            }
            
            attacking = false;
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;
            
            Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.AngleAxis(attackAngle, Vector3.forward), Vector3.one);
            Gizmos.DrawWireCube(new Vector2(attackSize.x/2, 0f), attackSize);
        }
    }
}