using System;
using UnityEngine;
using Disjointed.Tools.Extensions;
using Sprite = Disjointed.Sprites.Sprite;


namespace Disjointed.Environment
{
    public class Door : Sprite, IUnlockable
    {
        [Serializable] public class Data
        {
            public bool open;
            public bool locked;
        }

        public Data data;
        public LayerMask playerMask;
        
        new private Collider2D collider;
        
        override protected void Awake()
        {
            base.Awake();

            SetAnimationState("Open", data.open);
            collider = GetComponent<Collider2D>();
        }

        public void Lock() => data.locked = true;
        public void Unlock() => data.locked = false;
        public void ToggleLock() => data.locked = !data.locked;

        public void Open()
        {
            if (data.locked) return;
            
            data.open = true;
            collider.isTrigger = true;
            SetAnimationState("Open", data.open);
        }

        public void Close()
        {
            data.open = false;
            collider.isTrigger = false;
            SetAnimationState("Open", data.open);
        }

        public void ToggleOpen()
        {
            data.open = !data.open;
            collider.isTrigger = !collider.isTrigger;
            SetAnimationState("Open", data.open);
        }
    

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!playerMask.Contains(other.gameObject.layer)) return;

            Debug.Log("Player Touched Door!");
            if (!data.locked && !data.open) Open();
        }
    }
}
