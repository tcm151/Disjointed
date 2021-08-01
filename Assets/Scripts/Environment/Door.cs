using System;
using Disjointed.Tools.Extensions;
using UnityEngine;
using Sprite = Disjointed.Sprites.Sprite;


namespace Disjointed
{
    public class Door : Sprite
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

            collider = GetComponent<Collider2D>();
        }

        public void Lock() => data.locked = true;
        public void Unlock() => data.locked = false;
        
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

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!playerMask.Contains(other.gameObject.layer)) return;

            Debug.Log("Player Touched Door!");
            if (!data.locked && !data.open) Open();
        }
    }
}
