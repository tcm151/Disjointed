using UnityEngine;
using UnityEngine.UI;

namespace OGAM.SceneManagement
{
    public class QuitGameButton : MonoBehaviour
    {
        Button button;
        private void Awake() => button = GetComponent<Button>();

        private void OnMouseUpAsButton() => SceneSwitcher.ReloadScene();
    }
}
