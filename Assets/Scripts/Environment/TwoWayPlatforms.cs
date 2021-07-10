using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OGAM
{
    [RequireComponent(typeof(PlatformEffector2D))]
    public class TwoWayPlatforms : MonoBehaviour
    {
        public float timer;
        public float waitTime = 0.33f;
        public float resetTime = 0.1f;
        
        private PlatformEffector2D effector;
        
        private void Awake()
        {
            effector = GetComponent<PlatformEffector2D>();
            effector.surfaceArc = 140f;
            timer = 0f;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                timer = 0f;
            }
            
            if (Input.GetKey(KeyCode.S))
            {
                timer += Time.deltaTime;
                if (timer > waitTime) effector.surfaceArc = 0f;
            }
            
            if (Input.GetKeyUp(KeyCode.S))
            {
                timer = waitTime;
                StartCoroutine(CR_ResetPlatforms());
            }
        }

        private IEnumerator CR_ResetPlatforms()
        {
            yield return new WaitForSeconds(resetTime);
            effector.surfaceArc = 140f;
        }
    }
}