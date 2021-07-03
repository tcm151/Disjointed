using System;
using UnityEngine;

namespace OGAM
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;

        public float followDistance = 100f;
        public float followSpeed = 2.5f;
        
        new private Camera camera;

        private Vector2 screenCenter;
        private Vector2 targetScreenPosition;
        private Vector2 distanceFromCenter;
        private Vector2 velocity = Vector2.zero;

        private void Awake()
        {
            camera = GetComponent<Camera>();
            screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        }

        private void Update()
        {
            targetScreenPosition = camera.WorldToScreenPoint(target.position);
            distanceFromCenter = screenCenter - targetScreenPosition;
        }

        private void LateUpdate()
        {
            if (distanceFromCenter.magnitude > followDistance)
            {
                var targetPosition = target.position;
                targetPosition.z = -10f;
                transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
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
