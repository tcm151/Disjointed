using UnityEngine;

namespace OGAM
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;
        
        new private Camera camera;

        private Vector2 screenCenter;
        private Vector2 targetScreenPosition;
        private Vector2 distanceFromCenter;

        private void Awake()
        {
            camera = GetComponent<Camera>();
            
            screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            targetScreenPosition = camera.WorldToScreenPoint(target.position);
            distanceFromCenter = screenCenter - targetScreenPosition;
        }

        private void Update()
        {
            targetScreenPosition = camera.WorldToScreenPoint(target.position);
            distanceFromCenter = screenCenter - targetScreenPosition;
            
            // if (distanceFromCenter.magnitude > )
        }
    }
}
