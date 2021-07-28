﻿using UnityEngine;
using Disjointed.Combat;


namespace Disjointed.Environment
{
    public class Dangers : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision) => Damage(collision);
        private void OnCollisionStay2D(Collision2D collision) => Damage(collision);

        private void Damage(Collision2D collision)
        {
            var damageable = collision.collider.GetComponent<IDamageable>();
            damageable.TakeDamage(1, "Dangers!");

            var rigidbody = collision.collider.GetComponent<Rigidbody2D>();
            var newVelocity = -collision.contacts[0].normal * Mathf.Sqrt(2f * Physics2D.gravity.magnitude * rigidbody.gravityScale * 2.5f);
            Debug.Log(newVelocity);
            rigidbody.velocity = newVelocity;
        }
    }
}