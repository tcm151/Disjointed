using System;
using System.Collections;
using Disjointed.Combat;
using Disjointed.Environment;
using Disjointed.Tools.SceneManagement;
using UnityEngine;


namespace Disjointed.Player
{
    public class PlayerData : MonoBehaviour, IDamageable
    {
        [SerializeField] private Checkpoint lastCheckpoint;

        new private Rigidbody2D rigidbody;

        private int health;

        private bool invincible;
        public float invincibilityCooldown;

        public static Action<int> playerHealthChanged;

        private void Awake()
        {
            health = 3;
            invincible = false;
            rigidbody = GetComponent<Rigidbody2D>();
        }

        //> SET NEW CHECKPOINT
        public void SetCheckpoint(Checkpoint newCheckpoint) => lastCheckpoint = newCheckpoint;
        
        //> DEATH
        public void Die()
        {
            Debug.Log("You Died! :(");
            
            //@ revert to old save state and not just teleport 
            
            if (lastCheckpoint is { }) transform.position = lastCheckpoint.position;
            else SceneSwitcher.ReloadScene();
        }

        public void TakeDamage(int damage, string origin)
        {
            if (invincible) return;
            
            health -= damage;
            playerHealthChanged?.Invoke(health);
            if (health <= 0) Die();

            StartCoroutine(CR_Invincibility());
        }
        
        public void TakeKnockback(Vector2 direction, float knockback)
        {
            rigidbody.AddForce(direction * knockback, ForceMode2D.Impulse);
        }

        private IEnumerator CR_Invincibility()
        {
            invincible = true;
            yield return new WaitForSeconds(invincibilityCooldown);
            invincible = false;
        }
    }
}