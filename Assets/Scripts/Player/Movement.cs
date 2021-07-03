using System;
using UnityEngine;


namespace OGAM.Player
{
    public class Movement : MonoBehaviour
    {
        //+ COMPONENTS
        new private Rigidbody2D rigidbody;

        public LayerMask groundMask;
        public float movementSpeed = 4f;
        public float jumpHeight = 2.5f;

        //+ LOCAL STATE
        private Vector2 velocity;
        private bool jumping;
        private bool grounded;

        private const float GroundedDistance = 0.60f;

        //> INITIALIZATION
        private void Awake() => rigidbody = GetComponent<Rigidbody2D>();

        //> HANDLE INPUT
        private void Update()
        {
            // cache velocity for later
            velocity = rigidbody.velocity;
            
            jumping |= Input.GetKeyDown(KeyCode.Space);
            velocity.x = movementSpeed * Input.GetAxis("Horizontal");
        }

        //> HANDLE PHYSICS
        private void FixedUpdate()
        {
            // check if player is grounded
            var hit = Physics2D.Raycast(rigidbody.position, Vector2.down, GroundedDistance, groundMask);
            grounded = hit.collider is { };
            
            
            if (grounded && jumping)
            {
                jumping = false;
                velocity.y += Mathf.Sqrt(2f * Physics2D.gravity.magnitude * jumpHeight);
            }
            
            // apply the desired velocity to the player
            rigidbody.velocity = velocity;
        }

        //> DRAW HELPFUL GIZMOS
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;
            
            Gizmos.color = Color.green;
            Gizmos.DrawRay(rigidbody.position, Vector3.down * GroundedDistance);
        }
    }
}
