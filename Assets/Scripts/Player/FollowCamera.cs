using UnityEngine;

namespace OGAM.Player
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;

        public float followDistance = 100f;
        public float followSpeed = 2.5f;
        
        new private Camera camera;

        private Vector2 screenCenter;
        private Vector2 targetScreenPosition;
        private Vector2 distanceFromScreenCenter;

        private void Awake()
        {
            camera = GetComponent<Camera>();
            screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        }

        private void LateUpdate()
        {
            var targetPosition = target.position;
            targetScreenPosition = camera.WorldToScreenPoint(targetPosition);
            distanceFromScreenCenter = screenCenter - targetScreenPosition;

            if (distanceFromScreenCenter.magnitude > followDistance)
            {
                var cameraPosition = targetPosition + Vector3.back * 10;
                var smoothedPosition = Vector3.Slerp(transform.position, cameraPosition, followSpeed * Time.deltaTime);
                transform.position = smoothedPosition;
            }

            // camera.transform.position = Vector2.SmoothDamp(camera.transform.position, target.position, ref velocity, followSpeed);
        }
        
        

        // private void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.red;
        //     
        // }
    }
}
