using UnityEngine;
using UnityEngine.UI;

namespace OGAM.SceneManagement
{
    public class LoadSceneButton : MonoBehaviour
    {
        public int sceneToLoad;

        Button button;
        private void Awake() => button = GetComponent<Button>();
        
        private void OnMouseUpAsButton() => SceneSwitcher.LoadScene(sceneToLoad);
    }
}