using Disjointed.Game_Management;
using Disjointed.Tools.SceneManagement;
using UnityEngine;


namespace Disjointed.UI
{
    public class PauseWindow : UI_Window
    {
        public UI_Window HUD;
        
       
        
        override public void GoBack() => Hide();
        
        override protected void Awake()
        {
            base.Awake();
            Hide();
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;

            if (GameManager.IsPlaying) Pause();
            else Resume();
        }

        private void Pause()
        {
            GameManager.Pause();
            HUD.Hide();
            Show();
        }

        private void Resume()
        {
            Hide();
            HUD.Show();
            GameManager.Resume();
        }

        public void Quit()
        {
            GameManager.Quit();
        }

        public void Restart()
        {
            GameManager.Restart();
        }
    }
}
