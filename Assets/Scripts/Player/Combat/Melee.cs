﻿using System.Collections;
using UnityEngine;
using Disjointed.Combat;
using Disjointed.Tools.Extensions;
using Sprite = Disjointed.Sprites.Sprite;


namespace Disjointed.Player.Combat
{
    public class Melee : MonoBehaviour
    {
        public float damage = 1f;
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
        private void Awake()
        {
            camera = Camera.main;
            sprite = GetComponentInChildren<Sprite>();

            canAttack = true;
        }

        //> HANDLE INPUT
        private void Update()
        {
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
            sprite.TriggerAnimation("attacked");

            // cycle all overlap colliders
            var colliders = Physics2D.OverlapBoxAll(transform.position + (attackDirection * attackSize.x/2f), attackSize, attackAngle);
            foreach (var collider in colliders)
            {
                var damageable = collider.GetComponent<IDamageable>();
                if (damageable is null) continue;
                
                damageable.TakeDamage(damage, "Player Melee");

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