using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OGAM
{
    public class MouseFollow : MonoBehaviour
    {
        public enum MT
        {
            StayOnMouse,
            LerpToMouse,
            UsePhysics
        }
        public MT MoveType;

        public float lerpToMouseMaxDelta;
        public float physicsMovementForce;

        Rigidbody2D rb;
        public Camera mainCam;

        void Awake()
        {
            rb = this.GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            Vector3 mouseWorldPoint = mainCam.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPoint.z = 0f;

            switch (MoveType)
            {
                case MT.StayOnMouse:
                    rb.position = mouseWorldPoint; break;

                case MT.LerpToMouse:
                    rb.position = Vector2.Lerp(rb.position, mouseWorldPoint, lerpToMouseMaxDelta); break;

                case MT.UsePhysics:
                    Vector2 mouseDirection = mouseWorldPoint - this.transform.position;
                    rb.AddForce(mouseDirection * physicsMovementForce, ForceMode2D.Force); break;

                default:
                    rb.position = mouseWorldPoint; break;
            }
        }
    }
}
