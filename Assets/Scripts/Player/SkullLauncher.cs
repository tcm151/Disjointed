using UnityEngine;


namespace OGAM.Combat
{
    public class SkullLauncher : MonoBehaviour
    {
        public GameObject skullPrefab;
        
        public float launchSpeed = 5f;
        public Projectile.Data projectileData;
        
        [SerializeField] private bool hasSkull;

        new private Camera camera;

        private Vector3 mousePosition;
        private Vector2 mouseDirection;
        private bool firing;

        private void Awake()
        {
            camera = Camera.main;
        }

        private void Update()
        {
            firing |= Input.GetMouseButtonDown(1);

            mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            
            mouseDirection = (mousePosition - transform.position).normalized;
        }

        private void FixedUpdate()
        {
            if (!hasSkull || !firing) return;
            
            firing = false;
            
            var origin = transform.position;
            var projectile = Instantiate(skullPrefab, origin, Quaternion.identity).GetComponent<Projectile>();
            projectile.Launch(origin, mouseDirection, launchSpeed, projectileData);
        }
    }
    
}