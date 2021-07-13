using System;
using System.Collections;
using System.Collections.Generic;
using OGAM.Combat;
using UnityEngine;
using UnityEngine.Serialization;


namespace OGAM
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemy : MonoBehaviour, IDamageable
    {
        new private Rigidbody2D rigidbody;

        [FormerlySerializedAs("stats")][FormerlySerializedAs("ESO")] public EnemyData data;
        public float health;
        public float speed;
        public float knockbackMult;
        public Transform target;

        [Header("Ground/Wall Checking")]
        public LayerMask groundMask;
        public float groundedDistance = 0.85f;
        public bool onGround;
        public bool onWall;
        
        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            health = data.health;
            speed = data.speed;
            knockbackMult = data.knockbackMultiplier;
        }

        private void FixedUpdate()
        {
            Move();
        }

        public void Move()
        {
            var hit = Physics2D.Raycast(transform.position, Vector2.down, groundedDistance, groundMask);
            onGround = hit.collider is { };

            if (Mathf.Approximately(target.position.x, this.transform.position.x)) return;
            if (target.position.x > this.transform.position.x)
            {
                rigidbody.AddForce(Vector2.right * speed);
            }
            else rigidbody.AddForce(Vector2.right * -speed);
        }

        public void TakeDamage(float damage, string origin)
        {
            Debug.Log("DID DAMAGE!!!");
            health -= damage;
            
            if (health <= 0) Destroy(this.gameObject);
        }

        public void TakeKnockback(float knockback, Vector2 direction)
        {
            rigidbody.AddForce(direction * (knockback * knockbackMult), ForceMode2D.Impulse);
        }
    }
}
