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
        public enum Aggro
        {
            Ignore,
            Homing,      // Moves straight towards player with no regard for safety
            Intelligent, //Backs off when low on health, stays at optimal fighting distance
        }
        public enum Movement
        {
            Walking,
            Flying,
            Stationary,
        }
        
        
        [FormerlySerializedAs("template")]
        [Header("Enemy Template")]
        public EnemyData data;
        private float health;
        private float movementSpeed;
        private float acceleration;
        private float detectionRadius;
        private int damage;
        private float knockback;
        private Aggro aggro;
        private Movement movement;
        
        [Header("Target")]
        public Transform target;
        public LayerMask targetMask;
        public LayerMask detectionMask;

        [Header("Ground/Wall Checking")]
        public LayerMask groundMask;
        public float groundedDistance = 0.85f;
        public bool onGround;
        public bool onWall;

        
        new private Collider2D collider;
        new private Rigidbody2D rigidbody;
        private Sprite sprite;
        
        private Vector2 desiredVelocity;
        private Vector3 initialPosition;

        private void Awake()
        {
            sprite = GetComponent<Sprite>();
            collider = GetComponent<Collider2D>();
            rigidbody = GetComponent<Rigidbody2D>();
            
            health = data.health;
            acceleration = data.acceleration;
            movementSpeed = data.movementSpeed;
            detectionRadius = data.detectionRadius;
            damage = data.damage;
            knockback = data.knockback;

            aggro = data.aggro;
            movement = data.movement;

            initialPosition = transform.position;
        }

        private void Update()
        {
            if (rigidbody.velocity.x > 0.25f) sprite.FaceRight();
            if (rigidbody.velocity.x < -0.25f) sprite.FaceLeft();
        }

        private void FixedUpdate()
        {
            DetectPlayer();
            if (!target && Vector3.Distance(transform.position, initialPosition) < 0.25f)
            {
                rigidbody.velocity = Vector2.zero;
            }

            desiredVelocity = rigidbody.velocity;

            if (movement == Movement.Walking)
            {
                if (!target) return;
                
                var hit = Physics2D.Raycast(transform.position, Vector2.down, groundedDistance, groundMask);
                onGround = hit.collider is { };

                var targetDirection = (target.position - transform.position).normalized;
                desiredVelocity.x.MoveTowards(targetDirection.x * movementSpeed, acceleration * Time.deltaTime);
            }

            if (movement == Movement.Flying)
            {
                Vector3 targetDirection;
                if (target) targetDirection = (target.position - transform.position).normalized;
                else targetDirection = transform.position.DirectionTo(initialPosition);

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

            if (los.collider is { } && targetMask.Contains(los.collider.gameObject.layer)) target = detect.transform;
            else target = null;
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
