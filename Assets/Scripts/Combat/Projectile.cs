using System;
using UnityEngine;


namespace Disjointed.Combat
{
    [SelectionBase]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        //> PROJECTILE DATA CONTAINER
        [Serializable] public class Data
        {
            public string origin = "null";
            public int damage = 1;
            public float knockback = 1f;
        }
        
        private Data data;
        
        new private Rigidbody2D rigidbody;
        private Vector3 previousPosition;

        public LayerMask collisionMask;

        //> INITIALIZATION
        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            previousPosition = transform.position;
        }

        //> FIRE WITH A GIVEN VELOCITY
        public void Launch(Vector3 position, Vector3 direction, float speed, Data data)
        {
            this.data = data;
            rigidbody.position = position;
            rigidbody.AddForce(direction * (speed * rigidbody.mass), ForceMode2D.Impulse);
        }

        //> HANDLE PHYSICS & COLLISION DETECTION
        private void FixedUpdate()
        {
            if (rigidbody.velocity.magnitude > 0f) transform.right = rigidbody.velocity.normalized;

            var hit = Physics2D.Linecast(previousPosition, rigidbody.position, collisionMask);
            if (hit.collider is { })
            {
                IDamageable damageable = hit.collider.GetComponent<IDamageable>();
                damageable?.TakeDamage(data.damage, data.origin);
                damageable?.TakeKnockback(rigidbody.velocity.normalized, data.knockback);
        
                Destroy(this.gameObject);
            }
            else previousPosition = rigidbody.position;
        }
    }
}