using System;
using OGAM.Combat;
using OGAM.Tools;
using UnityEngine;


namespace OGAM.Player
{
    public class Combat : MonoBehaviour
    {
        private Vector2 previousAttackDirection;
        public Vector2 attackDirection;
        public Vector2 attackSize;
        public float attackAngle;

        public float damage = 1f;
        public float knockback = 10f;

        private bool attacking;

        private void Update()
        {
            attackDirection.x = Input.GetAxisRaw("Horizontal");
            attackDirection.y = Input.GetAxisRaw("Vertical");
            attackDirection.Normalize();

            if (attackDirection == Vector2.zero) attackDirection = previousAttackDirection;
            else previousAttackDirection = attackDirection;

            attackAngle = (attackDirection == Vector2.zero) ? attackAngle : attackDirection.Angle();
            
            attacking |= Input.GetMouseButtonDown(0);

        }

        private void FixedUpdate()
        {
            if (attacking)
            {
                attacking = false;

                var colliders = Physics2D.OverlapBoxAll(transform.position + new Vector3(attackDirection.x, attackDirection.y) * attackSize.x/2f, attackSize, attackAngle);
                foreach (var collider in colliders)
                {
                    var damageable = collider.GetComponent<IDamageable>();
                    if (damageable is { })
                    {
                        damageable.TakeDamage(damage, "melee");

                        var direction = (collider.transform.position - transform.position).normalized;
                        damageable.TakeKnockback(knockback, direction);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;
            
            Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.AngleAxis(attackAngle, Vector3.forward), Vector3.one);
            Gizmos.DrawWireCube(new Vector2(attackSize.x/2, 0f), attackSize);
            
            // Gizmos.DrawWireCube(transform.position + new Vector3(attackDirection.x, attackDirection.y) * attackSize.x/2f, attackSize);
        }
    }
}