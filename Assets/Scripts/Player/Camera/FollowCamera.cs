using UnityEngine;

namespace Disjointed.Player
{
    public class FollowCamera : MonoBehaviour
    {
        [Header("Target Settings")]
        [SerializeField] private Transform target;
        public float followDistance = 100f;
        public float followSpeed = 2.5f;
        
        new private Camera camera;
        private Vector2 screenCenter;
        private Vector2 targetScreenPosition;
        private Vector2 distanceFromScreenCenter;

        //> INITIALIZATION
        private void Awake()
        {
            camera = GetComponent<Camera>();
            screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        }

        //> UPDATE CAMERA POSITION
        private void LateUpdate()
        {
            if (!target) return;
            
            var targetPosition = target.position;
            targetScreenPosition = camera.WorldToScreenPoint(targetPosition);
            distanceFromScreenCenter = screenCenter - targetScreenPosition;

            if (distanceFromScreenCenter.magnitude > followDistance)
            {
                var cameraPosition = targetPosition + Vector3.back * 10;
                var smoothedPosition = Vector3.Slerp(transform.position, cameraPosition, followSpeed * Time.deltaTime);
                transform.position = smoothedPosition;
            }
        }

        public void SetTarget(Transform newTarget) => target = newTarget;
    }
}
