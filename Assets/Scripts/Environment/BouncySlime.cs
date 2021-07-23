using UnityEngine;


namespace Disjointed.Environment
{
    public class BouncySlime : MonoBehaviour
    {
        public float bounceMultiplier = 0.8f;
        
        private bool holdingShift;

        private void Update()
        {
            holdingShift = Input.GetKey(KeyCode.LeftShift);
        }

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