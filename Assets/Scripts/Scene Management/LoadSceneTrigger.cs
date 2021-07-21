using Disjointed.Tools;
using UnityEngine;

namespace Disjointed.SceneManagement
{
    public class LoadSceneTrigger : MonoBehaviour
    {
        public int sceneToLoad;
        public LayerMask playerMask;

        private void OnCollisionEnter(Collision collision)
        {
            if (playerMask.Contains(collision.gameObject.layer))
            {
                SceneSwitcher.LoadScene(sceneToLoad);
            }
        }
    }
}
