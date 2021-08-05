using System;
using Disjointed.Audio;
using UnityEngine;
using Disjointed.Combat;


namespace Disjointed.Environment
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Crate : MonoBehaviour, IDamageable
    {
        [Serializable] public class Data
        {
            public float health;
            [HideInInspector] public Vector3 position;
            [HideInInspector] public Vector3 rotation;
        }

        public Data data;
        new private Rigidbody2D rigidbody;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionStay(Collision other)
        {
            data.position = transform.position;
            data.rotation = transform.rotation.eulerAngles;
        }

        public void TakeDamage(float damage, string origin)
        {
            data.health -= damage;
            if (data.health <= 0)
            {
                Destroy(gameObject);
                AudioManager.Connect.PlayOneShot("CrateBreak");
            }
        }

        public void TakeKnockback(Vector2 direction, float knockback)
        {
            rigidbody.AddForce(direction * knockback, ForceMode2D.Impulse);
        }
    }
}