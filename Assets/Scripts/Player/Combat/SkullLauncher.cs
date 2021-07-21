using UnityEngine;
using Disjointed.Tools.ObjectCreation;


namespace Disjointed.Player.Combat
{
    public class SkullLauncher : MonoBehaviour
    {
        public Skull skullPrefab;
        public Skull.Data projectileData;
        public float launchSpeed = 5f;
        public bool hasSkull;


        new private Camera camera;
        private Vector3 mousePosition;
        private Vector2 mouseDirection;
        private bool firing;

        //> INITIALIZATION
        private void Awake() => camera = Camera.main;

        //> HANDLE INPUT
        private void Update()
        {
            firing |= Input.GetMouseButtonDown(1);

            mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            
            mouseDirection = (mousePosition - transform.position).normalized;
        }

        //> FIRE SKULLS
        private void FixedUpdate()
        {
            // return if no skull or not firing
            if (!hasSkull || !firing) return;
            
            var origin = transform.position;
            var projectile = Factory.Spawn(skullPrefab, origin); // create a new game object
            projectile.Launch(origin, mouseDirection, launchSpeed, projectileData);
            
            firing = false;
            hasSkull = false;
        }
    }
    
}
