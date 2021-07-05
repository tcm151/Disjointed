
using UnityEngine;


namespace OGAM.Combat
{
    [SelectionBase]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        //> PROJECTILE DATA CONTAINER
        [System.Serializable] public class Data
        {
            public string origin = "null";
            // public float life, expiry = 5f;
            public float mass = 1f;
            public float damage = 1f;
            public float knockback = 1f;
        }

        public LayerMask collisionMask;
        
        //- COMPONENTS
        new private Rigidbody2D rigidbody;

        //- STATE CHECKS
        // private bool IsMoving => rigidbody.velocity.magnitude > 0;
        // private bool IsExpired => data.life >= data.expiry;

        private Data data;
        private Vector3 previousPosition;

        //> INITIALIZATION
        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            previousPosition = rigidbody.position;
        }

        //> FIRE WITH A GIVEN DIRECTION AND SPEED
        public void Launch(Vector3 position, Vector3 direction, float speed, Data data)
            => Launch(position, direction * speed, data);

        //> FIRE WITH A GIVEN VELOCITY
        public void Launch(Vector3 position, Vector3 velocity, Data data)
        {
            this.data = data;
            rigidbody.position = previousPosition = position;
            rigidbody.AddForce(velocity * rigidbody.mass, ForceMode2D.Impulse);
        }

        //> UPDATE EVERY FRAME
        // private void Update()
        // {
        //     // if (IsExpired) Destroy(this.gameObject);
        //     // if (IsMoving) transform.rotation = Quaternion.LookRotation(rigidbody.velocity, Vector3.up);
        //
        //     CheckImpact();
        // }
        
        //> CHECK IMPACT FOR BALLISTIC PROJECTILES
        // private void CheckImpact()
        // {
        //     //@ ADD LAYER MASK CHECKING
        //     
        //     var hit = Physics2D.Linecast(previousPosition, rigidbody.position, collisionMask);
        //     // var hit = Physics2D.Linecast(previousPosition, rigidbody.position);
        //     
        //     if (hit.collider is { })
        //     {
        //         IDamageable damageable = hit.collider.GetComponent<IDamageable>();
        //         damageable?.TakeDamage(data.damage, data.knockback, data.origin);
        //
        //         ImpactAt(hit);
        //     }
        //     else previousPosition = rigidbody.position;
        // }

        //> DO THINGS ON IMPACT
        // private void ImpactAt(RaycastHit2D hit)
        // {
        //     rigidbody.velocity = Vector2.Reflect(rigidbody.velocity, hit.normal);
        // }
    }
}