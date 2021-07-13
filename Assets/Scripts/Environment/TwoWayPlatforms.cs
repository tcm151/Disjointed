using System.Collections;
using UnityEngine;

namespace OGAM.Environment
{
    [RequireComponent(typeof(PlatformEffector2D))]
    public class TwoWayPlatforms : MonoBehaviour
    {
        public float timer;
        public float waitTime = 0.33f;
        public float resetTime = 0.1f;
        
        private PlatformEffector2D effector;
        
        //> INITIALIZATION
        private void Awake()
        {
            effector = GetComponent<PlatformEffector2D>();
            effector.surfaceArc = 140f;
            timer = 0f;
        }

        //> HANDLE INPUT
        private void Update()
        {
            // start timer on key down
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
            yield return new WaitForSeconds(resetTime);
            effector.surfaceArc = 140f;
        }
    }
}