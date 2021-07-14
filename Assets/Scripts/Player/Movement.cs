using System;
using UnityEngine;


namespace OGAM.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Movement : MonoBehaviour
    {
        //- COMPONENTS
        new private Rigidbody2D rigidbody;
        new private SpriteRenderer renderer;

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
        private Vector2 desiredVelocity;
        private Vector2 contactNormal;
        [Header("Contact Checking")]
        public int timeSinceGrounded;
        public int timeSinceContact;
        public int timeSinceOnWall;
        public int contacts;
        [Header("State")]
        public bool jumping;
        public bool holdingJump;
        public bool onGround;
        public bool onWall;
        public bool wallJumping;
        
        //- HELPERS
        private float jumpSpeed => Mathf.Sqrt(2f * Physics2D.gravity.magnitude * jumpHeight);

        //> INITIALIZATION
        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            renderer  = GetComponentInChildren<SpriteRenderer>();
        }

        //> EVERY FRAME
        private void Update()
        {
            //+ HANDLE INPUT
            jumping |= Input.GetKeyDown(KeyCode.Space);
            holdingJump = Input.GetKey(KeyCode.Space);
            movementInput.x = Input.GetAxisRaw("Horizontal");
            movementInput.y = Input.GetAxisRaw("Vertical");

            // update direction of sprite
            if (rigidbody.velocity.x >  0.15f) renderer.flipX = false;
            if (rigidbody.velocity.x < -0.15f) renderer.flipX = true;
        }

        //> EVERY PHYSICS STEP
        private void FixedUpdate()
        {
            //+ UPDATE STATE
            timeSinceOnWall++; // wall separate buffer
            timeSinceContact++;// wall jump buffer
            timeSinceGrounded++; // jump buffer
            desiredVelocity = rigidbody.velocity; // cache current velocity
            rigidbody.gravityScale = ((holdingJump && desiredVelocity.y > 0f) || onWall) ? jumpGravity : fallGravity; // apply more gravity on fall
            if (timeSinceContact > 5) jumping = false; // cancel jump if not appropriate
            if (rigidbody.velocity.y < 2.25f) wallJumping = false;


            // allow player to jump thru platforms
            // Physics2D.IgnoreLayerCollision(PlayerLayer, PlatformLayer, rigidbody.velocity.y > 0.1f);

            //+ CHECK GROUNDED
            var hit = Physics2D.Raycast(transform.position, Vector2.down, groundedDistance, groundMask);
            if (hit.collider is { })
            {
                onGround = true;
                timeSinceContact = 0;
                timeSinceGrounded = 0;
            }
            else onGround = false;

            //+ HORIZONTAL MOVEMENT
            float maxDeltaSpeed = (onGround, onWall, movementInput.x == 0, timeSinceOnWall > 50, wallJumping) switch
            {
                (_,     true,  _,     false, _   ) => 0f,
                (false, false, false, _,     true) => 0f,
                (false, false, true,  _,     _   ) => maxDeceleration * Time.deltaTime,
                (true,  true,  _,     _,     _   ) => maxAcceleration * Time.deltaTime,
                (_,     _,     _,     _,     _   ) => maxAcceleration * Time.deltaTime,
            };
            desiredVelocity.x = Mathf.MoveTowards(desiredVelocity.x, movementInput.x * maxSpeed, maxDeltaSpeed);

            //+ REGULAR JUMPING
            if ((onGround || timeSinceGrounded < 5) && jumping)
            {
                jumping = false;
                desiredVelocity.y += jumpSpeed;
            }

            //+ WALL JUMPING
            if (onWall && jumping)
            {
                jumping = false;
                wallJumping = true;
                var jumpDirection = (contactNormal + Vector2.up).normalized;
                desiredVelocity = jumpDirection * jumpSpeed;
            }
            
            // clamp vertical velocity to avoid exploits
            // desiredVelocity.y = Mathf.Clamp(desiredVelocity.y, float.MinValue, jumpSpeed);

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
             if (contacts == 0) contactNormal = Vector2.zero;
             
             // fix weird edge case while on platforms
             contactNormal = (onGround) ? Vector2.up : Vector2.zero;
             
             // sum all of the contact normals 
             foreach (var contact in collision.contacts) contactNormal += contact.normal;
             contactNormal.Normalize(); // normalize to length 1
        
             // will be true if the player on in a wall
             onWall = (Vector2.Dot(contactNormal, Vector2.left) > 0.99f || Vector2.Dot(contactNormal, Vector2.right) > 0.99f);
             if (!onWall) timeSinceOnWall = 0;
         }


        //> DRAW HELPFUL GIZMOS
        private void OnDrawGizmos()
        {
            var transformPosition = transform.position;
            
            // Gizmos.color = Color.white;
            // Gizmos.DrawSphere(transformPosition, 0.1f);

            #if UNITY_EDITOR
            if (!Application.isPlaying) return;
            #endif
            
            // Gizmos.color = (onGround) ? Color.green : Color.red;
            // Gizmos.DrawRay(transformPosition, Vector3.down * groundedDistance);
            
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(transformPosition, contactNormal * 2f);
            
            // Gizmos.color = Color.blue;
            // Gizmos.DrawRay(transformPosition, rigidbody.velocity.normalized * 2f);
            
        }
    }
}
