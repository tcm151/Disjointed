using UnityEngine;
using UnityEngine.UI;

namespace OGAM.Environment
{
    public class SSViaButton : MonoBehaviour
    {
        public SceneSwitch Switcher;
        public int sceneToLoad;

        Button thisButton;
        private void Awake()
        { thisButton = GetComponent<Button>(); }

        private void OnMouseUpAsButton()
        {
            Switcher.SwitchScenes(sceneToLoad);
        }
    }
}
