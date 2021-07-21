#pragma warning disable 108,114

using UnityEngine;


namespace Disjointed.Sprites
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimation : MonoBehaviour
    {
        private Animator animator;
        private SpriteRenderer renderer;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            renderer = GetComponent<SpriteRenderer>();
        }

        public void PlayAnimation(string animation)
        {
            animator.Play(animation);
        }

        public void TriggerAnimation(string name)
        {
            animator.SetTrigger(name);
        }
    }
}