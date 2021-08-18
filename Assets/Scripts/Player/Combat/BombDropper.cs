using System;
using System.Collections;
using Disjointed.Tools.ObjectCreation;
using UnityEngine;


namespace Disjointed.Player.Combat
{
    public class BombDropper : MonoBehaviour
    {
        public Bomb bombPrefab;
        public int bombAmmo;

        new private Rigidbody2D rigidbody;
        private bool canBomb;

        private void Awake()
        {
            bombAmmo = 3;
            canBomb = true;

            rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (canBomb && Input.GetMouseButtonDown(2))
            {
                var bomb = Factory.Spawn(bombPrefab, transform.position);
                bomb.rigidbody.velocity = rigidbody.velocity;
                bombAmmo -= 1;
                canBomb = false;

                StartCoroutine(CR_BombCooldown());
            }
        }

        private IEnumerator CR_BombCooldown()
        {
            yield return new WaitForSeconds(10f);
            Debug.Log("NEW BOMB READY!");
            canBomb = true;
        }
    }
}