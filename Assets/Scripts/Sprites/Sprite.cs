#pragma warning disable 108,114
using UnityEngine;


namespace Disjointed.Sprites
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    abstract public class Sprite : MonoBehaviour
    {
        private Animator animator;
        private SpriteRenderer renderer;

        virtual protected void Awake()
        {
            animator = GetComponent<Animator>();
            renderer = GetComponent<SpriteRenderer>();
        }

        public void FaceUp() => renderer.flipY = false;
        public void FaceDown() => renderer.flipY = true;
        public void FaceLeft() => renderer.flipX = true;
        public void FaceRight() => renderer.flipX = false;

        public void TriggerAnimation(string name) => animator.SetTrigger(name);
        public void SetAnimationState(string name, bool value) => animator.SetBool(name, value);
    }
}