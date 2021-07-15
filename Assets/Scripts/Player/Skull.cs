using System;
using UnityEngine;
using OGAM.Combat;
using OGAM.Tools;


namespace OGAM.Player
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Skull : MonoBehaviour
    {
        //> PROJECTILE DATA CONTAINER
        [Serializable] public class Data
        {
            public string origin = "null";
            public float damage = 1f;
            public float knockback = 1f;
        }
        
        public Data data;
        public LayerMask playerMask;
        
        new private Rigidbody2D rigidbody;
        new private Collider2D collider;
        
        //> INITIALIZATION
        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            collider = GetComponent<Collider2D>();

            collider.isTrigger = true;
        }

        //> FIRE WITH A GIVEN VELOCITY
        public void Launch(Vector3 position, Vector3 direction, float speed, Data data)
        {
            this.data = data;
            rigidbody.position = position;
            rigidbody.AddForce(direction * (speed * rigidbody.mass), ForceMode2D.Impulse);
        }

        private void OnTriggerExit2D(Collider2D otherCollider)
        {
            if (!playerMask.Contains(otherCollider.gameObject.layer)) return;

            collider.isTrigger = false;
        }

        //> DO DAMAGE ON COLLISION
        private void OnCollisionEnter2D(Collision2D collision)
        {
            foreach (var contact in collision.contacts)
            {
                if (!playerMask.Contains(contact.collider.gameObject.layer)) continue;

                var skullLauncher = contact.collider.GetComponent<SkullLauncher>();
                if (skullLauncher is null) continue;

                skullLauncher.hasSkull = true;
                Destroy(this.gameObject);
            }
            
            var damageable = collision.gameObject.GetComponent<IDamageable>();
            damageable?.TakeDamage(data.damage, data.origin);
            damageable?.TakeKnockback(data.knockback, rigidbody.velocity.normalized);
        }
    }
}