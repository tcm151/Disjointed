using System;
using System.Collections;
using System.Linq;
using Disjointed.Combat;
using Disjointed.Tools.Extensions;
using UnityEngine;
using Sprite = Disjointed.Sprites.Sprite;


namespace Disjointed
{
    public class Bomb : Sprite
    {
        [Serializable] public class Data
        {
            public float damage;
            public float blastForce;
            public float blastRadius;
        }

        public Data data;
        public Sprite explosion;

        new public Rigidbody2D rigidbody => GetComponent<Rigidbody2D>();
        
        override protected void Awake()
        {
            base.Awake();
            StartCoroutine(CR_Explosion());
        }

        private IEnumerator CR_Explosion()
        {
            Debug.Log("Lighting Fuse!");
            
            TriggerAnimation("LightFuse");
            yield return new WaitForSeconds(2f);
            var colliders = Physics2D.OverlapCircleAll(transform.position, data.blastRadius).ToList();
            colliders.ForEach(c =>
            {
                var damageable = c.GetComponent<IDamageable>();
                damageable?.TakeDamage(data.damage, "bomb.");
                var direction = (transform.position.DirectionTo(c.transform.position));
                damageable?.TakeKnockback(direction, data.blastForce);
            });
            
            Debug.Log("EXPLOSION!");
            
            Destroy(gameObject);
        }
    }
}
