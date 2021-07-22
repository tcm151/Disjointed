using System;
using Disjointed.Combat;
using Disjointed.Tools.Extensions;
using UnityEngine;
using UnityEngine.Serialization;
using Sprite = Disjointed.Sprites.Sprite;


namespace Disjointed
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemy : MonoBehaviour, IDamageable
    {
        private Sprite sprite;
        new private Collider2D collider;
        new private Rigidbody2D rigidbody;

        public EnemyData data;
        public float health;
        public float movementSpeed;
        public float acceleration;
        public float knockbackMult;
        public Transform target;

        [Header("Ground/Wall Checking")]
        public LayerMask groundMask;
        public float groundedDistance = 0.85f;
        public bool onGround;
        public bool onWall;

        private Vector2 desiredVelocity;
        
        private void Awake()
        {
            sprite = GetComponent<Sprite>();
            collider = GetComponent<Collider2D>();
            rigidbody = GetComponent<Rigidbody2D>();
            
            health = data.health;
            acceleration = data.acceleration;
            movementSpeed = data.movementSpeed;
            knockbackMult = data.knockbackMultiplier;
        }

        private void Update()
        {
            if (rigidbody.velocity.x > 0.15f) sprite.FaceRight();
            if (rigidbody.velocity.x < -0.15f) sprite.FaceLeft();
        }

        private void FixedUpdate()
        {
            desiredVelocity = rigidbody.velocity;
            
            var hit = Physics2D.Raycast(transform.position, Vector2.down, groundedDistance, groundMask);
            onGround = hit.collider is { };

            var targetDirection = (target.position - transform.position).normalized;

            desiredVelocity.MoveTowards(targetDirection * movementSpeed, acceleration  * Time.deltaTime);
            
            rigidbody.velocity = desiredVelocity;
        }

        public void TakeDamage(float damage, string origin)
        {
            Debug.Log("DID DAMAGE!!!");
            health -= damage;
            
            if (health <= 0) Destroy(this.gameObject);
        }

        public void TakeKnockback(Vector2 direction, float knockback)
        {
            rigidbody.AddForce(direction * knockback, ForceMode2D.Impulse);
        }
    }
}
