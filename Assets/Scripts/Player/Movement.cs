using System.Collections.Generic;
using OGAM.Tools;
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
        public Vector3 groundedOffset;
        public float groundedDistance = 0.33f;
        [Header("Movement")]
        public float maxSpeed = 6f;
        public float maxAcceleration = 40f;
        public float maxDeceleration = 30f;
        public float minDeceleration = 5f;
        [Header("Jumping")]
        public float jumpHeight = 3.5f;
        public float regularGravity = 1f;
        public float fallGravity = 2f;
        public float wallGravity = 1f;

        //- LOCAL STATE
        private Vector2 movementInput;
        private Vector2 desiredVelocity;
        private Vector2 contactNormal;
        [Header("Contact Checking")]
        public int timeSinceGrounded;
        public int timeSinceJumping;
        public int timeSinceOnWall;
        public int contacts;
        
        [Header("On Wall")]
        public float maxWallFallSpeed;
        [Header("State")]
        public bool onGround;
        public bool onWall;
        public bool jumping;
        public bool holdingJump;
        public bool wallJumping;

        //- HELPERS
        private float jumpSpeed => Mathf.Sqrt(2f * Physics2D.gravity.magnitude * jumpHeight);

        //> INITIALIZATION
        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            renderer  = GetComponent<SpriteRenderer>();
        }

        //> EVERY FRAME
        private void Update()
        {
            //+ HANDLE INPUT
            jumping |= Input.GetKeyDown(KeyCode.Space);
            holdingJump = Input.GetKey(KeyCode.Space);
            movementInput.x = Input.GetAxisRaw("Horizontal");
            movementInput.y = Input.GetAxisRaw("Vertical");

            if (Input.GetKeyDown(KeyCode.Space)) timeSinceJumping = 0;

            // update direction of sprite
            if (rigidbody.velocity.x >  0.15f) renderer.flipX = false;
            if (rigidbody.velocity.x < -0.15f) renderer.flipX = true;
        }

        //> EVERY PHYSICS STEP
        private void FixedUpdate()
        {
            //+ UPDATE STATE
            timeSinceOnWall++; // wall separate buffer
            timeSinceJumping++; // allow for delayed jump
            timeSinceGrounded++; // jump buffer of ledges
            desiredVelocity = rigidbody.velocity; // cache current velocity
            
            //+ REVERT CONDITIONS
            if (timeSinceJumping > 5) jumping = false; // cancel jump if not appropriate
            if (rigidbody.velocity.y < 2.25f) wallJumping = false;
            
            

            // rigidbody.gravityScale = (onWall, holdingJump, desiredVelocity.y > 0f) switch
            // {
            //     (true, _, _) => wallGravity,
            //     (_, true, true) => regularGravity,
            //     (_, false, true) => fallGravity,
            //     (_, _, _) => regularGravity,
            //     
            // };
            rigidbody.gravityScale = ((holdingJump && desiredVelocity.y > 0f) || (onWall && desiredVelocity.y < 0f)) ? regularGravity : fallGravity; // apply more gravity on fall

            //+ CHECK GROUNDED
            var hit = Physics2D.CircleCast(transform.position + groundedOffset, 0.4f, Vector2.down, groundedDistance, groundMask);
            if (hit.collider is { })
            {
                onGround = true;
                timeSinceGrounded = 0;
            }
            else onGround = false;

            //+ HORIZONTAL MOVEMENT
            float acceleration = (onGround, onWall, movementInput.x == 0, timeSinceOnWall > 50, wallJumping) switch
            {
                (false, true,  _,     false, _   ) => 0f,
                (false, false, false, _,     true) => 0f,
                (false, false, true,  _,     _   ) => minDeceleration * Time.deltaTime,
                (true,  _,     true,  _,     _   ) => maxDeceleration * Time.deltaTime,
                (_,     _,     _,     _,     _   ) => maxAcceleration * Time.deltaTime,
            };
            desiredVelocity.x = Mathf.MoveTowards(desiredVelocity.x, movementInput.x * maxSpeed, acceleration);

            //+ REGULAR JUMPING
            if ((onGround || timeSinceGrounded < 5) && jumping)
            {
                jumping = false;
                desiredVelocity.y = jumpSpeed;
            }

            //+ WALL JUMPING
            if (onWall && jumping)
            {
                jumping = false;
                wallJumping = true;
                var jumpDirection = (contactNormal + Vector2.up).normalized;
                desiredVelocity = jumpDirection * jumpSpeed;
            }

            // avoid jumping exploits
            if (holdingJump) desiredVelocity.y = Mathf.Clamp(desiredVelocity.y, float.MinValue, jumpSpeed);
            
            // limit fall speed on wall
            if (onWall) desiredVelocity.y.Clamp(maxWallFallSpeed, float.MaxValue);

            // assign the final velocity
            rigidbody.velocity = desiredVelocity;
        }

         private void OnCollisionEnter2D(Collision2D collision) => ManageCollisions(collision);
         private void OnCollisionStay2D(Collision2D collision)  => ManageCollisions(collision);
         private void OnCollisionExit2D(Collision2D collision)  => ManageCollisions(collision);

        //> DETERMINE THE CONTACT NORMAL
        //@ Convert grounding checks into this, can't use raycasts anymore 
         private void ManageCollisions(Collision2D collision)
         {
             contactNormal = Vector2.zero; // reset contact normal

             // only calculate if touching something
             if ((contacts = collision.contactCount) > 0)
             {
                 // sum all of the contact normals 
                 foreach (var contact in collision.contacts) contactNormal += contact.normal;
                 contactNormal.Normalize(); // normalize to length 1

                 // project the contact normal onto the up direction
                 float dot = Vector2.Dot(contactNormal, Vector2.up);

                 // ground case
                 if (dot >= 0.55f)
                 {
                     onGround = true;
                     timeSinceGrounded = 0;
                 }
                 else onGround = false;

                 // wall case
                 if (dot < 0.55f && dot > -0.55f) onWall = true;
                 else
                 {
                     onWall = false;
                     timeSinceOnWall = 0;
                 } 
             }
             else
             {
                 onGround = onWall = false;
                 timeSinceOnWall = 0;
             }
         }


        //> DRAW HELPFUL GIZMOS
        private void OnDrawGizmos()
        {
            var transformPosition = transform.position;
            
            #if UNITY_EDITOR
            if (!Application.isPlaying) return;
            #endif
            
            Gizmos.color = (onGround) ? Color.green : Color.red;
            Gizmos.DrawRay(transformPosition + groundedOffset, Vector3.down * groundedDistance);
            
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(transformPosition, contactNormal);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transformPosition, rigidbody.velocity.normalized * 1.5f);
            
        }
    }
}
