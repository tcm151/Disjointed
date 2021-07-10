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
        public float waitTime = 0.5f;
        
        private PlatformEffector2D effector;
        
        private void Awake()
        {
            timer = 0f;
            effector = GetComponent<PlatformEffector2D>();
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
                if (timer > waitTime) effector.rotationalOffset = 180f;
            }
            
            if (Input.GetKeyUp(KeyCode.S))
            {
                timer = waitTime;
                StartCoroutine(CO_ResetPlatforms());
                // effector.rotationalOffset = 0f;
            }
        }

        private IEnumerator CO_ResetPlatforms()
        {
            yield return new WaitForSeconds(0.5f);
            effector.rotationalOffset = 0f;
        }
    }
}