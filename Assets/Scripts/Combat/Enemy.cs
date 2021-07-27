using System;
using UnityEngine;
using Disjointed.Combat;
using Disjointed.Player;
using Disjointed.Tools.Extensions;
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

        [Header("Target")]
        public Transform target;
        public LayerMask targetMask;
        public LayerMask detectionMask;
        
        [Header("Enemy Template")]
        public EnemyTemplate template;
        private float health;
        private float movementSpeed;
        private float acceleration;
        private float detectionRadius;
        private int damage;
        private float knockback;
        private EnemyTemplate.Aggro aggro;
        private EnemyTemplate.MovementType movementType;

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
            
            health = template.health;
            acceleration = template.acceleration;
            movementSpeed = template.movementSpeed;
            detectionRadius = template.detectionRadius;
            damage = template.damage;
            knockback = template.knockback;

            aggro = template.aggro;
            movementType = template.movementType;
        }

        private void Update()
        {
            if (rigidbody.velocity.x > 0.15f) sprite.FaceRight();
            if (rigidbody.velocity.x < -0.15f) sprite.FaceLeft();
        }

        private void FixedUpdate()
        {
            DetectPlayer();
            if (!target) return;
            
            desiredVelocity = rigidbody.velocity;

            if (movementType == EnemyTemplate.MovementType.Walking)
            {
                var hit = Physics2D.Raycast(transform.position, Vector2.down, groundedDistance, groundMask);
                onGround = hit.collider is { };

                var targetDirection = (target.position - transform.position).normalized;
                desiredVelocity.x.MoveTowards(targetDirection.x * movementSpeed, acceleration * Time.deltaTime);
            }

            if (movementType == EnemyTemplate.MovementType.Flying)
            {
                var targetDirection = (target.position - transform.position).normalized;
                desiredVelocity.MoveTowards(targetDirection * movementSpeed, acceleration  * Time.deltaTime);
            }
            
            rigidbody.velocity = desiredVelocity;
        }

        private void DetectPlayer()
        {
            var detect = Physics2D.OverlapCircle(transform.position, detectionRadius, targetMask);
            if (detect is null) return;
            
            var targetDirection = detect.transform.position - transform.position;
            var los = Physics2D.Raycast(transform.position, targetDirection, detectionRadius, detectionMask);

            if (los.collider is { } && targetMask.Contains(los.collider.gameObject.layer))
                target = detect.transform;
        }

        public void TakeDamage(int damage, string origin)
        {
            health -= damage;
            if (health <= 0) Destroy(this.gameObject);
        }

        public void TakeKnockback(Vector2 direction, float knockback)
        {
            rigidbody.AddForce(direction * knockback, ForceMode2D.Impulse);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable is null) return;

            var direction = collision.transform.position - transform.position;
            
            damageable.TakeDamage(damage, "Enemy!");
            damageable.TakeKnockback(direction, knockback);
            
            rigidbody.AddForce(-direction * knockback, ForceMode2D.Impulse);
        }

        private void OnDrawGizmos()
        {
            #if UNITY_EDITOR
            if (!Application.isPlaying) return;
            #endif
            
            var position = transform.position;
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(position, detectionRadius);
            
            if (!target) return;
            
            Gizmos.color = Color.green;
            Gizmos.DrawLine(position, target.position);
        }
    }
}
