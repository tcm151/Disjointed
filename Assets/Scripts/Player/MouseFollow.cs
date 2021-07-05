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
           UsePhysics,
           Vector2MoveToMouse
        }
        public MT MoveType;

        public float lerpToMouseMaxDelta;
        public float physicsMovementForce;
        public float maxSpeed = 3f;
    
        Rigidbody2D rb;
        public Camera mainCam;

        // Start is called before the first frame update
        void Awake()
        {
            rb = this.GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Vector3 mouseworldpoint = mainCam.ScreenToWorldPoint(Input.mousePosition);
            mouseworldpoint.z = 0f;

            switch (MoveType)
            {
                case MT.StayOnMouse:
                    rb.position = mouseworldpoint; break;

                case MT.LerpToMouse:
                    rb.position = Vector2.Lerp(rb.position, mouseworldpoint, lerpToMouseMaxDelta); break;

                case MT.UsePhysics:
                    Vector2 mouseDirection = mouseworldpoint - this.transform.position;
                    float leForce = physicsMovementForce * Vector2.Distance(rb.position, mouseworldpoint);
                    Vector2 maxForce = mouseDirection * Mathf.Clamp(leForce, 0f, maxSpeed);
                    rb.AddForce(maxForce, ForceMode2D.Force); break;

                case MT.Vector2MoveToMouse:
                    rb.position = Vector2.MoveTowards(rb.position, mouseworldpoint, maxSpeed); break;

                default:
                    rb.position = mouseworldpoint; break;
            }
        }
    }
}
