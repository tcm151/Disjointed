using System;
using System.Linq;
using UnityEngine;


namespace OGAM.Environment
{
    public class BouncySlime : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            var rigidbody = collision.collider.GetComponent<Rigidbody2D>();
            if (rigidbody is null) return;

            Debug.Log("HIT THE SLIME!");

            var contact = collision.GetContact(0);
            Debug.Log(collision.contactCount);

            Debug.Log(contact.normalImpulse);
            Debug.Log(contact.relativeVelocity);
            
            var exitVelocity = Vector2.Reflect(contact.relativeVelocity, contact.normal);

            Debug.Log(exitVelocity);
            
            rigidbody.velocity = exitVelocity;
        }
    }
}