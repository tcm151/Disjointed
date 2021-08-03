using System;
using UnityEngine;
using Disjointed.Tools.Extensions;
using Sprite = Disjointed.Sprites.Sprite;


namespace Disjointed.Environment
{
    public class Lever : Sprite
    {
        public MonoBehaviour linkedObject;
        public LayerMask playerMask;

        private IUnlockable unlockable;
        
        private bool ePressed;
        
        override protected void Awake()
        {
            base.Awake();

            unlockable = linkedObject.GetComponent<IUnlockable>();
            if (unlockable is null) Debug.LogError("Assigned Unlockable was not correct!");
        }

        
        private void Update()
        {
            ePressed |= Input.GetKeyDown(KeyCode.E);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!playerMask.Contains(other.gameObject.layer)) return;
            InteractionPrompt.onShowPrompt?.Invoke();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!playerMask.Contains(other.gameObject.layer)) return;
            InteractionPrompt.onHidePrompt?.Invoke();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (playerMask.Contains(other.gameObject.layer))
            {
                if (ePressed)
                {
                    Debug.Log("Toggled Door!");
                    unlockable.ToggleLock();
                }
            }
            ePressed = false;
        }
    }
}