using System;
using System.Collections;
using UnityEngine;
using Disjointed.Combat;
using Disjointed.Environment;
using Disjointed.Tools.SceneManagement;
using Disjointed.Tools.Serialization;


namespace Disjointed.Player
{
    public class ThePlayer : MonoBehaviour, IDamageable
    {
        [Serializable] public class Data
        {
            public float health;
            public Vector3 position;

        }

        public Serializer serializer;

        public Data data;

        private Checkpoint lastCheckpoint;
        new private Rigidbody2D rigidbody;
        
        private bool invincible;
        public float invincibilityCooldown;

        public static Action<float> healthChanged;
        public static Action playerDeath;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            
            Initialize();
        }

        private void Initialize()
        {
            data.health = 3;
            healthChanged?.Invoke(data.health);
            invincible = false;
        }

        private void FixedUpdate()
        {
            data.position = transform.position;
        }

        //> SET NEW CHECKPOINT
        public void SetCheckpoint(Checkpoint newCheckpoint) => lastCheckpoint = newCheckpoint;
        
        //> DEATH
        public void Die()
        {
            Debug.Log("You Died! :(");

            serializer.LoadGame();
        }

        //> TAKE INCOMING DAMAGE
        public void TakeDamage(float damage, string origin)
        {
            if (invincible) return;
            
            data.health -= damage;
            healthChanged?.Invoke(data.health);
            if (data.health <= 0) Die();

            // StartCoroutine(CR_Invincibility());
        }
        
        //> TAKE KNOCKBACK
        public void TakeKnockback(Vector2 direction, float knockback)
        {
            rigidbody.AddForce(direction * knockback, ForceMode2D.Impulse);
        }

        //> INVINCIBILITY PERIOD
        private IEnumerator CR_Invincibility()
        {
            invincible = true;
            yield return new WaitForSeconds(invincibilityCooldown);
            invincible = false;
        }
    }
}