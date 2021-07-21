using Disjointed.Combat;
using UnityEngine;
using UnityEngine.Serialization;


namespace Disjointed
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemy : MonoBehaviour, IDamageable
    {
        new private Rigidbody2D rigidbody;

        [FormerlySerializedAs("stats")][FormerlySerializedAs("ESO")] public EnemyData data;
        public float health;
        public float acceleration;
        public float topSpeed;
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
            acceleration = data.acceleration;
            topSpeed = data.topSpeed;
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
                if (rigidbody.velocity.magnitude > topSpeed)
                {
                    rigidbody.AddForce(Vector2.right);
                }
                else
                {
                    rigidbody.AddForce(Vector2.right * acceleration);
                }
            }
            else
            {
                if (rigidbody.velocity.magnitude > topSpeed)
                {
                    rigidbody.AddForce(Vector2.right * -1);
                }
                else
                {
                    rigidbody.AddForce(Vector2.right * -acceleration);
                }
            }

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
