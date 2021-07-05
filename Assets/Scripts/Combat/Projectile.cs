
using System;
using UnityEngine;


namespace OGAM.Combat
{
    [SelectionBase]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        //> PROJECTILE DATA CONTAINER
        [Serializable] public class Data
        {
            public string origin = "null";
            public float damage = 1f;
            public float knockback = 1f;
        }
        
        new private Rigidbody2D rigidbody;
        // public LayerMask collisionMask;
        private Data data;

        //> INITIALIZATION
        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        //> FIRE WITH A GIVEN VELOCITY
        public void Launch(Vector3 position, Vector3 direction, float speed, Data data)
        {
            this.data = data;
            rigidbody.position = position;
            rigidbody.AddForce(direction * speed, ForceMode2D.Impulse);
        }

        //> DO DAMAGE ON COLLISION
        private void OnCollisionEnter2D(Collision2D collision)
        {
            var damageable = collision.gameObject.GetComponent<IDamageable>();
            damageable?.TakeDamage(data.damage, data.knockback, data.origin);
        }
    }
}