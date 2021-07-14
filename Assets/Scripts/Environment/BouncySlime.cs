using System;
using System.Linq;
using UnityEngine;


namespace OGAM.Environment
{
    public class BouncySlime : MonoBehaviour
    {
        public float bounceMultiplier = 0.8f;
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // find rigidbodies
            var rigidbody = collision.collider.GetComponent<Rigidbody2D>();
            if (rigidbody is null) return;
            
            // bounce rigidbody off slime
            var contact = collision.GetContact(0);
            var exitVelocity = Vector2.Reflect(contact.relativeVelocity, contact.normal);
            exitVelocity *= bounceMultiplier;

            // apply reflected velocity
            rigidbody.velocity = exitVelocity;
        }
    }
}