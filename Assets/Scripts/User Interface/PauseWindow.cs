using Disjointed.Tools.SceneManagement;
using UnityEngine;


namespace Disjointed.UI
{
    public class PauseWindow : UI_Window
    {
        public UI_Window HUD;
        
        private static bool Paused => Time.timeScale == 0f;
        
        override public void GoBack() => Hide();
        
        override protected void Awake()
        {
            base.Awake();
            Hide();
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;

            if (Paused) Resume();
            else Pause();
        }

        public void Restart()
        {
            Resume();
            SceneSwitcher.ReloadScene();
        }

        public void Quit() => SceneSwitcher.QuitGame();

        public void Resume()
        {
            Hide();
            Time.timeScale = 1f;
            HUD.Show();
        }

        public void Pause()
        {
            HUD.Hide();
            Time.timeScale = 0f;
            Show();
        }
    }
}
