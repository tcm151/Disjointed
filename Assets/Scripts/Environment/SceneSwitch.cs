using UnityEngine;
using UnityEngine.SceneManagement;

namespace OGAM.Environment
{
    public class SceneSwitch : MonoBehaviour
    {
        public void SwitchScenes(int toThis)
        {
            SceneManager.LoadScene(toThis); //For other scripts to access
        }
    }
}
