using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OGAM
{
    public class MouseFollow : MonoBehaviour
    {
        public enum MoveType
        {
            StayOnMouse,
            LerpToMouse,
            UsePhysics,
        }
        public MoveType moveType;

        public float lerpToMouseMaxDelta = 5f;
        public float physicsMovementForce = 100f;

        new private Camera camera;
        new private Rigidbody2D rigidbody;

        private void Awake()
        {
            camera = Camera.main;
            rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            Vector3 mouseWorldPoint = camera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPoint.z = 0f;

            if (Input.GetMouseButtonDown(1))
            {
                // toggle the parrot and make sure that it rests on top of the player character
            }

            switch (moveType)
            {
                case MoveType.StayOnMouse:
                    rigidbody.position = mouseWorldPoint; break;

                case MoveType.LerpToMouse:
                    rigidbody.position = Vector2.Lerp(rigidbody.position, mouseWorldPoint, lerpToMouseMaxDelta * Time.deltaTime); break;

                case MoveType.UsePhysics:
                    Vector2 mouseDirection = mouseWorldPoint - transform.position;
                    rigidbody.AddForce(mouseDirection * (physicsMovementForce * Time.deltaTime), ForceMode2D.Force); break;

                default:
                    rigidbody.position = mouseWorldPoint; break;
            }
        }
    }
}
