using UnityEngine;

namespace Disjointed.ThePlayer
{
    public class MouseFollow : MonoBehaviour
    {
        public enum MoveType
        {
            StayOnMouse,
            LerpToMouse,
            UsePhysics,
            Vector2MoveToMouse,
        }
        public MoveType moveType;

        public float lerpToMouseMaxDelta;
        public float physicsMovementForce;
        public float maxSpeed = 3f;
    
        new private Camera camera;
        new private Rigidbody2D rigidbody;

        private void Awake()
        {
            camera = Camera.main;
            rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            Vector3 mouseWorldPoint = camera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPoint.z = 0f;

            switch (moveType)
            {
                case MoveType.StayOnMouse:
                    rigidbody.position = mouseWorldPoint;
                    break;

                case MoveType.LerpToMouse:
                    rigidbody.position = Vector2.Lerp(rigidbody.position, mouseWorldPoint, lerpToMouseMaxDelta);
                    break;

                case MoveType.UsePhysics:
                    Vector2 mouseDirection = mouseWorldPoint - transform.position;
                    float leForce = physicsMovementForce * Vector2.Distance(rigidbody.position, mouseWorldPoint);
                    Vector2 maxForce = mouseDirection * Mathf.Clamp(leForce, 0f, maxSpeed);
                    rigidbody.AddForce(maxForce, ForceMode2D.Force);
                    break;

                case MoveType.Vector2MoveToMouse:
                    rigidbody.position = Vector2.MoveTowards(rigidbody.position, mouseWorldPoint, maxSpeed);
                    break;
            }
        }
    }
}
