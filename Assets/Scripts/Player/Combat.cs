using System;
using OGAM.Combat;
using OGAM.Tools;
using UnityEngine;


namespace OGAM.Player
{
    public class Combat : MonoBehaviour
    {
        public Vector2 attackDirection;
        public Vector2 attackSize;
        public float attackAngle;

        private bool attacking;

        private void Update()
        {
            attackDirection.x = Input.GetAxisRaw("Horizontal");
            attackDirection.y = Input.GetAxisRaw("Vertical");
            attackDirection.Normalize();

            attackAngle = (attackDirection == Vector2.zero) ? attackAngle : attackDirection.Angle();
            
            attacking |= Input.GetMouseButtonDown(0);

        }

        private void FixedUpdate()
        {
            if (attacking)
            {
                attacking = false;

                var colliders = Physics2D.OverlapBoxAll(transform.position + new Vector3(attackDirection.x * attackSize.x/2, attackDirection.y), attackSize, attackAngle);
                foreach (var collider in colliders)
                {
                    var damageable = collider.GetComponent<IDamageable>();
                    if (damageable is { })
                    {
                        damageable.TakeDamage(1, "melee");

                        var direction = (collider.transform.position - transform.position).normalized;
                        damageable.TakeKnockback(1, direction);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.AngleAxis(attackAngle, Vector3.forward), Vector3.one);
            Gizmos.DrawWireCube(new Vector2(attackSize.x/2, 0f), attackSize);
        }
    }
}