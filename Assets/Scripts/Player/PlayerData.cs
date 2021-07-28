using System;
using System.Collections;
using UnityEngine;
using Disjointed.Combat;
using Disjointed.Environment;
using Disjointed.Tools.SceneManagement;


namespace Disjointed.Player
{
    public class PlayerData : MonoBehaviour, IDamageable
    {
        [SerializeField] private Checkpoint lastCheckpoint;

        new private Rigidbody2D rigidbody;

        private int health;

        private bool invincible;
        public float invincibilityCooldown;

        public static Action<int> healthChanged;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            
            Initialize();
        }

        private void Initialize()
        {
            health = 3;
            healthChanged?.Invoke(health);
            invincible = false;
        }

        //> SET NEW CHECKPOINT
        public void SetCheckpoint(Checkpoint newCheckpoint) => lastCheckpoint = newCheckpoint;
        
        //> DEATH
        public void Die()
        {
            Debug.Log("You Died! :(");
            
            //@ revert to old save state and not just teleport 
            
            Initialize();
            
            if (lastCheckpoint is { }) transform.position = lastCheckpoint.position;
            else SceneSwitcher.ReloadScene();
        }

        //> TAKE INCOMING DAMAGE
        public void TakeDamage(int damage, string origin)
        {
            if (invincible) return;
            
            health -= damage;
            healthChanged?.Invoke(health);
            if (health <= 0) Die();

            StartCoroutine(CR_Invincibility());
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