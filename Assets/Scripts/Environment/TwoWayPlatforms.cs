using System.Collections;
using UnityEngine;

namespace OGAM.Environment
{
    [RequireComponent(typeof(PlatformEffector2D))]
    public class TwoWayPlatforms : MonoBehaviour
    {
        private PlatformEffector2D effector;
        private float timer;
        
        public float waitTime = 0.33f;
        public float resetTime = 0.1f;
        public float surfaceArc = 140f;
        
        //> INITIALIZATION
        private void Awake()
        {
            effector = GetComponent<PlatformEffector2D>();
            effector.surfaceArc = surfaceArc;
            timer = 0f;
        }

        //> HANDLE INPUT
        private void Update()
        {
            // reset timer on key down
            if (Input.GetKeyDown(KeyCode.S))
            {
                timer = 0f;
            }

            // tick timer while key held
            if (Input.GetKey(KeyCode.S))
            {
                timer += Time.deltaTime;
                if (timer > waitTime) effector.surfaceArc = 0f;
            }

            // start reset cycle on key up
            if (Input.GetKeyUp(KeyCode.S))
            {
                timer = waitTime;
                StartCoroutine(CR_ResetPlatforms());
            }
        }

        //> WAIT FOR RESET
        private IEnumerator CR_ResetPlatforms()
        {
            // wait for 0.1f seconds
            yield return new WaitForSeconds(resetTime);
            
            // then reset the arc
            effector.surfaceArc = surfaceArc;
        }
    }
}