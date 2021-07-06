using System;
using System.Collections;
using System.Collections.Generic;
using OGAM.Combat;
using UnityEngine;

namespace OGAM
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemy : MonoBehaviour, IDamageable
    {
        new private Rigidbody2D rigidbody;

        public float health = 2f;
        
        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        public void TakeDamage(float damage, string origin)
        {
            Debug.Log("DID DAMAGE!!!");
            health -= damage;
            
            if (health <= 0) Destroy(this.gameObject);
        }

        public void TakeKnockback(float knockback, Vector2 direction)
        {
            rigidbody.AddForce(direction * knockback, ForceMode2D.Impulse);
        }
    }
}
