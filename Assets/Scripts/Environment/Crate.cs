using System;
using Disjointed.Combat;
using UnityEngine;


namespace Disjointed.Environment
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Crate : MonoBehaviour, IDamageable
    {
        new private Rigidbody2D rigidbody;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        public void TakeDamage(float damage, string origin)
        {
            // naww maybe break later
        }

        public void TakeKnockback(Vector2 direction, float knockback)
        {
            rigidbody.AddForce(direction * knockback, ForceMode2D.Impulse);
        }
    }
}