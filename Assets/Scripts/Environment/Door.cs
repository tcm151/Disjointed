using System;
using Disjointed.Audio;
using Disjointed.Combat;
using UnityEngine;
using Disjointed.Tools.Extensions;
using Sprite = Disjointed.Sprites.Sprite;


namespace Disjointed.Environment
{
    public class Door : Sprite, IUnlockable, IDamageable
    {
        //> DOOR DATA STRUCT
        [Serializable] public class Data
        {
            public bool open;
            public bool locked;
            public float health;
        }

        public Data data;
        public LayerMask playerMask;

        public float defaultHealth = 5f;
        
        new private Collider2D collider;
        
        //> INITIALIZATION
        override protected void Awake()
        {
            base.Awake();

            data.health = defaultHealth;
            SetAnimationState("Open", data.open);
            collider = GetComponent<Collider2D>();
        }

        //> LOCK THE DOOR
        public void Lock()
        {
            data.locked = true;
            AudioManager.PlaySFX?.Invoke("Lock");
        }

        //> UNLOCK THE DOOR
        public void Unlock()
        {
            data.locked = false;
            AudioManager.PlaySFX?.Invoke("Unlock");
        }

        //> TOGGLE THE DOOR LOCKED/UNLOCKED
        public void ToggleLock()
        {
            data.locked = !data.locked;
            AudioManager.PlaySFX?.Invoke("Lock");
        }

        //> OPEN THE DOOR
        public void Open()
        {
            if (data.locked) return;
            
            data.open = true;
            collider.isTrigger = true;
            AudioManager.PlaySFX?.Invoke("WoodenDoorOpen");
            SetAnimationState("Open", data.open);
        }

        //> CLOSE THE DOOR
        public void Close()
        {
            data.open = false;
            collider.isTrigger = false;
            AudioManager.PlaySFX?.Invoke("WoodenDoorClose");
            SetAnimationState("Open", data.open);
        }

        //> TOGGLE THE DOOR OPEN/CLOSED
        public void ToggleOpen()
        {
            data.open = !data.open;
            collider.isTrigger = !collider.isTrigger;
            AudioManager.PlaySFX?.Invoke("WoodenDoorOpen");
            SetAnimationState("Open", data.open);
        }
    
        //> OPEN DOOR ON COLLISION
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!playerMask.Contains(other.gameObject.layer)) return;
            if (!data.locked && !data.open) Open();
        }

        public void TakeDamage(float damage, string origin)
        {
            data.health -= damage;
            if (data.health <= 0f)
            {
                AudioManager.PlaySFX?.Invoke("CrateBreak");
                Destroy(gameObject);
            }
        }

        public void TakeKnockback(Vector2 direction, float knockback) { }
    }
}
