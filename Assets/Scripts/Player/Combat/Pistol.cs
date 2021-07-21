using System;
using System.Collections;
using System.Collections.Generic;
using Disjointed.Combat;
using Disjointed.Tools;
using UnityEngine;

namespace Disjointed
{
    public class Pistol : MonoBehaviour
    {
        public Projectile bulletPrefab;
        public Projectile.Data projectileData;

        [Header("Properties")]
        public float launchSpeed = 25f;
        public float reloadTime = 1f;
        public int ammoQuantity = 1;

        [Header("Recoil")]
        public float recoilForce = 100f;

        new private Rigidbody2D rigidbody;
        new private Camera camera;

        private Vector2 mouseDirection;
        private bool canFire;
        private bool firing;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            camera = Camera.main;
            
            canFire = true;
        }

        private void Update()
        {
            firing |= (canFire && Input.GetMouseButtonDown(0));
            
            var mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            mouseDirection = (mousePosition - transform.position).normalized;
        }

        private void FixedUpdate()
        {
            if (!firing || ammoQuantity < 1) return;
            
            canFire = firing = false;
            
            var origin = transform.position;
            var bullet = Factory.Spawn(bulletPrefab, origin); // create a new game object
            bullet.Launch(origin, mouseDirection, launchSpeed, projectileData);

            rigidbody.AddForce(-mouseDirection * recoilForce, ForceMode2D.Impulse);

            StartCoroutine(CR_Reload());
        }

        private IEnumerator CR_Reload()
        {
            yield return new WaitForSeconds(reloadTime);
            canFire = true;
        }
    }
}
