using System;
using UnityEngine;
using Disjointed.Audio;
using Disjointed.Tools.Extensions;
using Sprite = Disjointed.Sprites.Sprite;


namespace Disjointed.Combat.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    abstract public class Enemy : Sprite, IDamageable
    {
        //> ENEMY ENUMS
        public enum Type {Bat, Rat, Ghost }
        public enum Aggro { Ignore, Charge, Intelligent }
        
        //> ENEMY DATA STRUCT
        [Serializable] public class Data
        {
            public Type type;
            
            [Header("Movement")]
            public float acceleration = 20f;
            public float movementSpeed = 2.5f;

            [Header("Combat")]
            public Aggro aggro;
            public float health = 5f;
            public float damage = 1f;
            public float knockback = 5f;
            public float detectionRadius = 8f;

            [HideInInspector] public Vector3 position;
        }

        

        [Header("Enemy Properties")]
        public Data data;

        [Header("Target")] public Transform target;
        public LayerMask targetMask;
        public LayerMask detectionMask;

        [Header("Ground/Wall Checking")] public LayerMask groundMask;
        public float groundedDistance = 0.85f;
        public bool onGround;
        public bool onWall;

        new protected Rigidbody2D rigidbody;

        protected Vector2 desiredVelocity;
        protected Vector3 initialPosition;
        
        public bool IsMoving => (rigidbody.velocity.magnitude > 1f);

        abstract protected void Move();
        
        //> INITIALIZATION
        override protected void Awake()
        {
            base.Awake();
            rigidbody = GetComponent<Rigidbody2D>();
            initialPosition = transform.position;
        }

        //> UPDATE SPRITE DIRECTION
        virtual protected void Update()
        {
            if (rigidbody.velocity.x > 0.25f) FaceRight();
            if (rigidbody.velocity.x < -0.25f) FaceLeft();
        }


        //> DETECT IF PLAYER WITHIN RANGE & LINE OF SIGHT
        virtual protected void DetectPlayer()
        {
            var currentPosition = transform.position;
            
            var detect = Physics2D.OverlapCircle(currentPosition, data.detectionRadius, targetMask);
            if (detect is null) return;

            var targetDirection = detect.transform.position - currentPosition;
            var los = Physics2D.Raycast(currentPosition, targetDirection, data.detectionRadius, detectionMask);

            if (los.collider is { } && targetMask.Contains(los.collider.gameObject.layer)) target = detect.transform;
            else target = null;
        }

        //> TAKE DAMAGE
        virtual public void TakeDamage(float damage, string origin)
        {
            data.health -= damage;
            AudioManager.PlaySFX?.Invoke("SwordHitEnemy");
            if (data.health <= 0) Destroy(this.gameObject);
        }

        //> TAKE KNOCKBACK
        virtual public void TakeKnockback(Vector2 direction, float knockback)
            => rigidbody.AddForce(direction * knockback, ForceMode2D.Impulse);

        //> DEAL DAMAGE ON COLLISION
        virtual protected void OnCollisionEnter2D(Collision2D collision)
        {
            var damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable is null) return;

            damageable.TakeDamage(data.damage, "Enemy!");

            var direction = collision.transform.position - transform.position;
            damageable.TakeKnockback(direction, data.knockback);
        }

        //> DRAW HELPFUL GIZMOS
        virtual protected void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            var position = transform.position;

            Gizmos.color = Color.grey;
            Gizmos.DrawWireSphere(position, data.detectionRadius);

            if (!target) return;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(position, target.position);
        }
    }
}