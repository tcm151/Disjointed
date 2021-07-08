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
        public float jumpForce;
        public float jumpGravity = 1f;
        public float fallGravity = 2f;

        //- LOCAL STATE
        private Vector2 movementInput;
        private Vector2 contactNormal;
        private Vector2 desiredVelocity;
        private int timeSinceGrounded;
        private bool jumping;
        private bool holdingJump;
        private bool grounded;
        private bool onWall;

        //- CONSTANTS
        public float GroundedDistance = 0.85f;
        private static readonly Vector3 FaceRight = new Vector3( 1, 1, 1);
        private static readonly Vector3 FaceLeft  = new Vector3(-1, 1, 1);

        //> INITIALIZATION
        private void Awake() => rigidbody = GetComponent<Rigidbody2D>();

        //> HANDLE INPUT
        private void Update()
        {
            // cache desiredVelocity for later
            desiredVelocity = rigidbody.velocity;
            
            // get movement input
            jumping |= Input.GetKeyDown(KeyCode.Space);
            holdingJump = Input.GetKey(KeyCode.Space);
            rigidbody.gravityScale = (holdingJump) ? jumpGravity : fallGravity;
            
            movementInput.x = movementSpeed * Input.GetAxis("Horizontal");
            movementInput.y = movementSpeed * Input.GetAxis("Vertical");

            if (movementInput.x > 0) transform.localScale = FaceRight;
            if (movementInput.x < 0) transform.localScale = FaceLeft;

            //@ temporary would like to slow down the player instead of stopping immediately 
            desiredVelocity.x = movementInput.x;
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
                rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                Debug.Log("JUMPED!");
            }

            if (onWall && jumping)
            {
                jumping = false;
                var jumpDirection = (contactNormal + Vector2.up).normalized;
                rigidbody.AddForce(jumpDirection * jumpForce / 2f, ForceMode2D.Impulse);
                Debug.Log("JUMPED!");
            }

            // assign the final velocity
            rigidbody.velocity = desiredVelocity;
        }

        private void OnCollisionStay2D(Collision2D collision) => ManageCollisions(collision);
        private void OnCollisionExit2D(Collision2D collision) => ManageCollisions(collision);

        //? WORK IN PROGRESS
        private void ManageCollisions(Collision2D collision)
        {
            contactNormal = Vector2.zero;

            foreach (var contact in collision.contacts)
            {
                contactNormal += contact.normal;
            }
            
            contactNormal.Normalize();

            onWall = (Vector2.Dot(contactNormal, Vector2.up) > -0.1f);

            Debug.DrawRay(rigidbody.position, contactNormal, Color.magenta);
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
