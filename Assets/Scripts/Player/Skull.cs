﻿using System;
using UnityEngine;
using OGAM.Combat;


namespace OGAM.Player
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Skull : MonoBehaviour
    {
        //> PROJECTILE DATA CONTAINER
        [Serializable] public class Data
        {
            public string origin = "null";
            public float mass = 1f;
            public float damage = 1f;
            public float knockback = 1f;
        }
        
        public Data data;
        
        new private Rigidbody2D rigidbody;
        // private Vector3 previousPosition; 

        //> INITIALIZATION
        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.mass = data.mass;
        }

        //> FIRE WITH A GIVEN VELOCITY
        public void Launch(Vector3 position, Vector3 direction, float speed, Data data)
        {
            this.data = data;
            rigidbody.mass = data.mass;
            rigidbody.position = position;
            rigidbody.AddForce(direction * (speed * data.mass), ForceMode2D.Impulse);
        }
        
        // //> CHECK IMPACT FOR BALLISTIC PROJECTILES
        // virtual protected void CheckImpact()
        // {
        //     if (Physics.Linecast(previousPosition, rigidbody.position, out RaycastHit hit))
        //     {
        //         IDamageable damageable = hit.collider.GetComponent<IDamageable>();
        //         damageable?.TakeDamage(data.damage, data.origin);
        //         damageable?.TakeKnockback(data.knockback, rigidbody.velocity.normalized);
        //
        //         Destroy(this.gameObject);
        //     }
        //     else previousPosition = rigidbody.position;
        // }

        //> DO DAMAGE ON COLLISION
        private void OnCollisionEnter2D(Collision2D collision)
        {
            var damageable = collision.gameObject.GetComponent<IDamageable>();
            damageable?.TakeDamage(data.damage, data.origin);
            damageable?.TakeKnockback(data.knockback, rigidbody.velocity.normalized);
        }
    }
}