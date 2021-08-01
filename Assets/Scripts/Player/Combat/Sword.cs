using System.Collections;
using UnityEngine;
using Disjointed.Audio;
using Disjointed.Combat;
using Disjointed.Tools.Extensions;
using Disjointed.Tools.GameManagement;
using Sprite = Disjointed.Sprites.Sprite;


namespace Disjointed.Player.Combat
{
    public class Sword : Sprite
    {
        public LayerMask enemyMask;
        public int damage = 1;
        public float knockback = 10f;
        public float attackCooldown;
        public Vector2 attackSize;

        new private Camera camera;
        private Sprite sprite;
        
        private Vector3 lastAttackDirection;
        private Vector3 attackDirection;
        private float attackAngle;
        private bool attacking;
        private bool canAttack;

        //> INITIALIZATION
        override protected void Awake()
        {
            base.Awake();
            
            camera = Camera.main;
            sprite = GetComponentInChildren<Sprite>();

            canAttack = true;
        }

        //> HANDLE INPUT
        private void Update()
        {
            if (GameManager.IsPaused) return;
        
            attacking |= (canAttack && Input.GetMouseButtonDown(0));
            
            // attackDirection.x = Input.GetAxisRaw("Horizontal");
            // attackDirection.y = Input.GetAxisRaw("Vertical");
            // attackDirection.Normalize();

            var mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            attackDirection = (mousePosition - transform.position).normalized;
            
            if (attackDirection == Vector3.zero) attackDirection = lastAttackDirection;
            else lastAttackDirection = attackDirection;

            var dot = Vector2.Dot(attackDirection, Vector2.right);
            
            if (dot >= 0) sprite.FaceUp();
            else sprite.FaceDown();

            attackAngle = (attackDirection == Vector3.zero) ? attackAngle : attackDirection.Angle();
        }

        //> ATTACK
        private void FixedUpdate()
        {
            // ignore if not attacking
            if (!attacking) return;

            canAttack = attacking = false;

            sprite.transform.rotation = Quaternion.AngleAxis(attackAngle, Vector3.forward);
            sprite.TriggerAnimation("Attack");
            
            AudioManager.Connect.PlayOneShot("SwordSwipe");

            StartCoroutine(CR_Attack());
        }

        private IEnumerator CR_Attack()
        {
            yield return new WaitForSeconds(0.1f);
            
            // cycle all overlap colliders
            var colliders = Physics2D.OverlapBoxAll(transform.position + (attackDirection * attackSize.x/2f), attackSize, attackAngle, enemyMask);
            foreach (var collider in colliders)
            {
                var damageable = collider.GetComponent<IDamageable>();
                if (damageable is null) continue;
                
                damageable.TakeDamage(damage, "ThePlayer Sword");

                var direction = (collider.transform.position - transform.position).normalized;
                damageable.TakeKnockback(direction, knockback);
            }

            StartCoroutine(CR_AttackCooldown());
        }
        
        private IEnumerator CR_AttackCooldown()
        {
            yield return new WaitForSeconds(attackCooldown);
            canAttack = true;
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;
            
            Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.AngleAxis(attackAngle, Vector3.forward), Vector3.one);
            Gizmos.DrawWireCube(new Vector2(attackSize.x/2, 0f), attackSize);
        }
    }
}