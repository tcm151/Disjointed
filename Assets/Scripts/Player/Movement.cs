using System;
using UnityEngine;


namespace OGAM.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Movement : MonoBehaviour
    {
        //- COMPONENTS
        new private Transform transform;
        new private Rigidbody2D rigidbody;
        new private SpriteRenderer renderer;

        //- TWEAKABLES
        [Header("Ground Checking")]
        public LayerMask groundMask;
        public float groundedDistance = 0.85f;
        [Header("Movement")]
        public float maxSpeed = 4f;
        public float maxAcceleration = 10f;
        public float maxDeceleration = 25f;
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
        private const int PlayerLayer = 8;
        private const int PlatformLayer = 12;

        //> INITIALIZATION
        private void Awake()
        {
            transform = GetComponent<Transform>();
            rigidbody = GetComponent<Rigidbody2D>();
            renderer  = GetComponent<SpriteRenderer>();
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
            if (rigidbody.velocity.x >  0.1f) renderer.flipX = false;
            if (rigidbody.velocity.x < -0.1f) renderer.flipX = true;
        }

        //> HANDLE PHYSICS
        private void FixedUpdate()
        {
            //+ UPDATE STATE
            timeSinceContact++; // wall jump buffer
            timeSinceGrounded++; // jump buffer
            desiredVelocity = rigidbody.velocity; // cache current velocity
            rigidbody.gravityScale = (holdingJump && desiredVelocity.y > 0f) ? jumpGravity : fallGravity; // apply more gravity on fall
            if (timeSinceContact > 10) jumping = false; // cancel jump if not appropriate

            // allows player to jump thru platforms
            Physics2D.IgnoreLayerCollision(PlayerLayer, PlatformLayer, rigidbody.velocity.y > 0.1f);

            //+ CHECK GROUNDED
            var hit = Physics2D.Raycast(rigidbody.position, Vector2.down, groundedDistance, groundMask);
            if (hit.collider is { })
            {
                grounded = true;
                timeSinceContact = 0;
                timeSinceGrounded = 0;
            }
            else grounded = false;

            //+ HORIZONTAL MOVEMENT
            if (grounded || movementInput.x != 0)
            {
                var maxDeltaSpeed = maxAcceleration * Time.deltaTime;
                desiredVelocity.x = Mathf.MoveTowards(desiredVelocity.x, movementInput.x * maxSpeed, maxDeltaSpeed);
            }
            if (!grounded && movementInput.x == 0)
            {
                var maxDeltaSpeed = maxDeceleration * Time.deltaTime;
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
        
        //> DETERMINE THE CONTACT NORMAL
         private void ManageCollisions(Collision2D collision)
         {
             timeSinceContact = 0; // reset on contact
             contactNormal = Vector2.zero; // reset normal
             contacts = collision.contactCount; // count number of contact
             
             // if not contacting anything, normal is up
             if (contacts == 0) contactNormal = Vector2.up;
        
             // sum all of the contact normals 
             foreach (var contact in collision.contacts) contactNormal += contact.normal;
             contactNormal.Normalize(); // normalize to length 1
        
             // will be true if the player on in a wall
             onWall = (contactNormal == Vector2.left || contactNormal == Vector2.right);
         }


        //> DRAW HELPFUL GIZMOS
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            var transformPosition = transform.position;
            
            Gizmos.color = (grounded) ? Color.green : Color.red;
            Gizmos.DrawRay(transformPosition, Vector3.down * groundedDistance);
            
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(transformPosition, contactNormal);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transformPosition, rigidbody.velocity.normalized * 2f);
        }
    }
}
