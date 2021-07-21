using UnityEngine;

using Disjointed.Tools;
using Sprite = Disjointed.Sprites.Sprite;


namespace Disjointed.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlatformerMovement : MonoBehaviour
    {
        //- COMPONENTS
        private Sprite sprite;
        new private Rigidbody2D rigidbody;

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
        [Header("Ladders")]
        public LayerMask ladderMask;
        
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
        public bool onLadder;

        //- HELPERS
        private float jumpSpeed => Mathf.Sqrt(2f * Physics2D.gravity.magnitude * rigidbody.gravityScale * jumpHeight);

        //> INITIALIZATION
        private void Awake()
        {
            sprite = GetComponent<Sprite>();
            rigidbody = GetComponent<Rigidbody2D>();
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
            if (rigidbody.velocity.x > 0.15f) sprite.FaceRight();
            if (rigidbody.velocity.x < -0.15f) sprite.FaceLeft();
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
            // if (onGround) timeSinceJumping = 0;
            if (timeSinceJumping > 5) jumping = false; // cancel jump if not appropriate
            if (rigidbody.velocity.y < 2.25f) wallJumping = false;
            
            rigidbody.gravityScale = (holdingJump, onWall, onLadder, desiredVelocity.y > 0f) switch
            {
                (true, _,   _,    true ) => regularGravity,
                (_,   true, _,    false) => wallGravity,
                (_,   _,    true, _    ) => 0f,
                (_, _, _, _) => fallGravity,
                
            };
            // rigidbody.gravityScale = ((holdingJump && desiredVelocity.y > 0f) || (onWall && desiredVelocity.y < 0f)) ? regularGravity : fallGravity; // apply more gravity on fall

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
            desiredVelocity.x.MoveTowards(movementInput.x * maxSpeed, acceleration);

            //+ VERTICAL MOVEMENT
            if (onLadder && timeSinceJumping > 25)
            {
                acceleration = maxDeceleration * Time.deltaTime;
                desiredVelocity.x.MoveTowards(movementInput.x * maxSpeed, acceleration);
                desiredVelocity.y.MoveTowards(movementInput.y * maxSpeed, acceleration);
            }

            //+ REGULAR JUMPING
            if ((onGround || timeSinceGrounded < 5) && jumping)
            {
                Debug.Log("REGULAR JUMP!");
                jumping = false;
                rigidbody.gravityScale = regularGravity;
                desiredVelocity.y = jumpSpeed;
            }

            //+ WALL JUMPING
            if (onWall && jumping)
            {
                Debug.Log("WALL JUMP!");
                jumping = false;
                wallJumping = true;
                rigidbody.gravityScale = regularGravity;
                var jumpDirection = (contactNormal + Vector2.up).normalized;
                desiredVelocity = jumpDirection * jumpSpeed;
            }
            
            //+ LADDER JUMPING
            if (onLadder && jumping)
            {
                Debug.Log("LADDER JUMP!");
                jumping = false;
                onLadder = false;
                rigidbody.gravityScale = regularGravity;
                desiredVelocity.y = jumpSpeed;
            }
            
            // if (timeSinceJumping < 25) onLadder = false;

            
            // avoid jumping exploits
            if (holdingJump) desiredVelocity.y.Clamp(float.MinValue, jumpSpeed);
            
            // limit fall speed on wall
            if (onWall) desiredVelocity.y.Clamp(maxWallFallSpeed, float.MaxValue);
            
            // assign the final velocity
            rigidbody.velocity = desiredVelocity;
        }

        private void OnTriggerStay2D(Collider2D collider)
        {
            onLadder = ladderMask.Contains(collider.gameObject.layer) && timeSinceJumping > 25;
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (ladderMask.Contains(collider.gameObject.layer))
            {
                onLadder = false;
            }
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

                 // player is on the wall if conditions met
                 onWall = (dot < 0.55f && dot > -0.55f);
             }
             else onWall = false;
             
             // reset timer if not on wall
             if (!onWall) timeSinceOnWall = 0;
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
