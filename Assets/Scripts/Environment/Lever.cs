using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Disjointed.Audio;
using UnityEngine;
using Disjointed.Tools.Editor;
using Disjointed.Tools.Extensions;
using UnityEngine.Serialization;
using Sprite = Disjointed.Sprites.Sprite;


namespace Disjointed.Environment
{
    public class Lever : Sprite
    {
        [RequireInterface(typeof(IUnlockable))] public List<GameObject> linkedUnlockables;

        public LayerMask playerMask;

        private List<IUnlockable> unlockables;
        private bool ePressed;
        private bool activated;

        override protected void Awake()
        {
            base.Awake();

            SetAnimationState("Activated", activated);

            unlockables = linkedUnlockables.Select(u => u.GetComponent<IUnlockable>()).ToList();
            if (unlockables.Count == 0) Debug.LogError("Assigned Unlockable was not correct!");
        }

        private void Update()
        {
            ePressed |= Input.GetKeyDown(KeyCode.E);
            if (ePressed) StartCoroutine(CR_ResetState());
        }

        private IEnumerator CR_ResetState()
        {
            yield return new WaitForSeconds(0.1f);
            ePressed = false;
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
                    Debug.Log("Toggled Lever!");
                    AudioManager.onPlaySFX?.Invoke("LeverFlick");
                    unlockables.ForEach
                    (
                        u =>
                        {
                            u.ToggleLock();
                            u.ToggleOpen();
                        }
                    );

                    activated = !activated;
                    SetAnimationState("Activated", activated);
                }
            }

            ePressed = false;
        }
    }
}