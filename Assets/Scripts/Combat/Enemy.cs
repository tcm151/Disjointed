using System;
using UnityEngine;
using Disjointed.Audio;
using Disjointed.Tools.Extensions;
using Disjointed.Tools.Serialization;
using Sprite = Disjointed.Sprites.Sprite;


namespace Disjointed.Combat.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    abstract public class Enemy : Sprite, IDamageable
    {
        //> ENEMY DATA STRUCT
        [Serializable] public class Data : ISerializeable
        {
            [Header("Movement")]
            public Movement movement;
            public float acceleration = 20f;
            public float movementSpeed = 2.5f;
            
            [Header("Combat")]
            public Aggro aggro;
            public float health = 5f;
            public float damage = 1f;
            public float knockback = 5f;
            public float detectionRadius = 8f;

            public void Save() { }
            public void Load() { }
        }
        
        public enum Aggro { Ignore, Charge, Intelligent }
        public enum Movement { Walking, Flying, Stationary }

        public bool IsMoving => (rigidbody.velocity.magnitude > 1f);
        
        [Header("Enemy Properties")]
        public Data data;

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
        
        private Vector2 desiredVelocity;
        private Vector3 initialPosition;

        override protected void Awake()
        {
            base.Awake();
            
            collider = GetComponent<Collider2D>();
            rigidbody = GetComponent<Rigidbody2D>();

            initialPosition = transform.position;
        }

        virtual protected void Update()
        {
            if (rigidbody.velocity.x > 0.25f) FaceRight();
            if (rigidbody.velocity.x < -0.25f) FaceLeft();
        }

        virtual protected void FixedUpdate()
        {
            DetectPlayer();
            if (!target && Vector3.Distance(transform.position, initialPosition) < 0.25f)
            {
                rigidbody.velocity = Vector2.zero;
            }

            desiredVelocity = rigidbody.velocity;

            if (data.movement == Movement.Walking)
            {
                if (!target) return;
                
                var hit = Physics2D.Raycast(transform.position, Vector2.down, groundedDistance, groundMask);
                onGround = hit.collider is { };

                var targetDirection = (target.position - transform.position).normalized;
                desiredVelocity.x.MoveTowards(targetDirection.x * data.movementSpeed, data.acceleration * Time.deltaTime);
            }

            if (data.movement == Movement.Flying)
            {
                if (target)
                {
                    var targetDirection = (target.position - transform.position).normalized;
                    desiredVelocity.MoveTowards(targetDirection * data.movementSpeed, data.acceleration  * Time.deltaTime);
                }
                else
                {
                    var targetDirection = transform.position.DirectionTo(initialPosition);
                    if (Vector2.Distance(transform.position, initialPosition) < 0.1f) transform.position = initialPosition;
                    else desiredVelocity.MoveTowards(targetDirection * data.movementSpeed, data.acceleration  * Time.deltaTime);
                }

                
            }
            
            rigidbody.velocity = desiredVelocity;
        }

        virtual protected void DetectPlayer()
        {
            var detect = Physics2D.OverlapCircle(transform.position, data.detectionRadius, targetMask);
            if (detect is null) return;
            
            var targetDirection = detect.transform.position - transform.position;
            var los = Physics2D.Raycast(transform.position, targetDirection, data.detectionRadius, detectionMask);

            if (los.collider is { } && targetMask.Contains(los.collider.gameObject.layer)) target = detect.transform;
            else target = null;
        }

        virtual public void TakeDamage(float damage, string origin)
        {
            data.health -= damage;
            AudioManager.Connect.PlayOneShot("ZombieOof");
            if (data.health <= 0) Destroy(this.gameObject);
        }

        virtual public void TakeKnockback(Vector2 direction, float knockback)
        {
            rigidbody.AddForce(direction * knockback, ForceMode2D.Impulse);
        }

        virtual protected void OnCollisionEnter2D(Collision2D collision)
        {
            var damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable is null) return;

            damageable.TakeDamage(data.damage, "Enemy!");
            
            var direction = collision.transform.position - transform.position;
            damageable.TakeKnockback(direction, data.knockback);
            
            rigidbody.AddForce(-direction * data.knockback, ForceMode2D.Impulse);
        }

        virtual protected void OnDrawGizmos()
        {
            #if UNITY_EDITOR
            if (!Application.isPlaying) return;
            #endif
            
            var position = transform.position;
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(position, data.detectionRadius);
            
            if (!target) return;
            
            Gizmos.color = Color.green;
            Gizmos.DrawLine(position, target.position);
        }
    }
}
