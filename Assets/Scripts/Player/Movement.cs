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
        [Header("Ground Checking")]
        public LayerMask groundMask;
        public float groundedDistance = 0.85f;
        [Header("Movement")]
        public float maxSpeed = 4f;
        public float maxAcceleration = 10f;
        [Header("Jumping")]
        public float jumpHeight = 2.5f;
        public float jumpGravity = 1f;
        public float fallGravity = 2f;

        //- LOCAL STATE
        private Vector2 movementInput;
        private Vector2 contactNormal;
        private Vector2 desiredVelocity;
        private int timeSinceGrounded;
        private int timeSinceContact;
        private bool jumping;
        private bool holdingJump;
        public bool grounded;
        public bool onWall;
        public int contacts;

        //- CONSTANTS
        private static readonly Vector3 FaceRight = new Vector3( 1, 1, 1);
        private static readonly Vector3 FaceLeft  = new Vector3(-1, 1, 1);

        //> INITIALIZATION
        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        //> HANDLE INPUT
        private void Update()
        {
            // get input
            jumping |= Input.GetKeyDown(KeyCode.Space);
            holdingJump = Input.GetKey(KeyCode.Space);
            movementInput.x = Input.GetAxisRaw("Horizontal");
            movementInput.y = Input.GetAxisRaw("Vertical");

            // update direction of sprite
            if (rigidbody.velocity.x > 0) transform.localScale = FaceRight;
            if (rigidbody.velocity.x < 0) transform.localScale = FaceLeft;
        }

        //> HANDLE PHYSICS
        private void FixedUpdate()
        {
            //+ UPDATE STATE
            timeSinceContact++;
            timeSinceGrounded++; // jump buffer
            desiredVelocity = rigidbody.velocity; // cache current velocity
            rigidbody.gravityScale = (holdingJump) ? jumpGravity : fallGravity; // apply more gravity on fall
            if (timeSinceContact > 10) jumping = false;

            //+ CHECK GROUNDED
            var hit = Physics2D.Raycast(rigidbody.position, Vector2.down, groundedDistance, groundMask);
            if (hit.collider is { })
            {
                grounded = true;
                timeSinceGrounded = 0;
            }
            else grounded = false;

            //+ HORIZONTAL MOVEMENT
            if (grounded || movementInput.x != 0)
            {
                var maxDeltaSpeed = maxAcceleration * Time.deltaTime;
                desiredVelocity.x = Mathf.MoveTowards(desiredVelocity.x, movementInput.x * maxSpeed, maxDeltaSpeed);
            }
            
            // jump if the player is grounded & trying to jump
            if ((grounded || timeSinceGrounded < 5) && jumping)
            {
                jumping = false;     // use this formula to get exact jump height
                desiredVelocity.y += Mathf.Sqrt(2f * Physics2D.gravity.magnitude * jumpHeight);
                // Debug.Log("REGULAR JUMP!");
            }

            if ((onWall || timeSinceContact < 5) && jumping)
            {
                jumping = false;
                var jumpDirection = (contactNormal + Vector2.up).normalized;
                desiredVelocity += jumpDirection * Mathf.Sqrt(2f * Physics2D.gravity.magnitude * jumpHeight);
                // Debug.Log("WALL JUMP!");
            }

            desiredVelocity.y = Mathf.Clamp(desiredVelocity.y, float.MinValue, Mathf.Sqrt(2f * Physics2D.gravity.magnitude * jumpHeight));

            // assign the final velocity
            rigidbody.velocity = desiredVelocity;
        }

         private void OnCollisionStay2D(Collision2D collision) => ManageCollisions(collision);
         private void OnCollisionExit2D(Collision2D collision) => ManageCollisions(collision);
        
        //? WORK IN PROGRESS
         private void ManageCollisions(Collision2D collision)
         {
             timeSinceContact = 0;
             contactNormal = Vector2.zero;
             contacts = collision.contactCount;
        
             foreach (var contact in collision.contacts) contactNormal += contact.normal;

             contactNormal.Normalize();
        
             onWall = (contactNormal == Vector2.left || contactNormal == Vector2.right);
        
             Debug.DrawRay(rigidbody.position, contactNormal, Color.magenta);
         }


        //> DRAW HELPFUL GIZMOS
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            Gizmos.color = (grounded) ? Color.green : Color.red;
            Gizmos.DrawRay(rigidbody.position, Vector3.down * groundedDistance);
        }
    }
}
