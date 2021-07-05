using System;
using UnityEngine;


namespace OGAM.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Movement : MonoBehaviour
    {
        //- COMPONENTS
        new private Rigidbody2D rigidbody;

        //- TWEAKABLES
        public LayerMask jumpingMask;
        public float movementSpeed = 4f;
        public float jumpHeight = 2.5f;

        //- LOCAL STATE
        private Vector2 desiredVelocity;
        private int timeSinceGrounded;
        private bool jumping;
        private bool grounded;

        //- CONSTANTS
        private const float GroundedDistance = 0.60f;

        //> INITIALIZATION
        private void Awake() => rigidbody = GetComponent<Rigidbody2D>();

        //> HANDLE INPUT
        private void Update()
        {
            // cache desiredVelocity for later
            desiredVelocity = rigidbody.velocity;
            
            // get movement input
            jumping |= Input.GetKeyDown(KeyCode.Space);
            desiredVelocity.x = movementSpeed * Input.GetAxis("Horizontal");
        }

        //> HANDLE PHYSICS
        private void FixedUpdate()
        {
            timeSinceGrounded++; // give the player a jump buffer to jump
            
            // check if player is grounded
            var hit = Physics2D.Raycast(rigidbody.position, Vector2.down, GroundedDistance, jumpingMask);
            if (hit.collider is { })
            {
                grounded = true;
                timeSinceGrounded = 0;
            }
            else grounded = false;

            // jump if the player is grounded & trying to jump
            if ((grounded || timeSinceGrounded < 5) && jumping)
            {
                jumping = false;     // use this formula to get exact jump height
                desiredVelocity.y += Mathf.Sqrt(2f * Physics2D.gravity.magnitude * jumpHeight);
            }

            // assign the final velocity
            rigidbody.velocity = desiredVelocity;
        }

        //> DRAW HELPFUL GIZMOS
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            Gizmos.color = (grounded) ? Color.green : Color.red;
            Gizmos.DrawRay(rigidbody.position, Vector3.down * GroundedDistance);
        }
    }
}
