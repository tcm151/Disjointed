using System;
using OGAM.Environment;
using UnityEngine;

namespace OGAM
{
    public class PauseWindow : UI_Window
    {
        override public void GoBack() => Hide();

        private static bool Paused => Time.timeScale == 0f;
        
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

        public void Restart() => SceneSwitcher.ReloadScene();

        public void Quit() => SceneSwitcher.QuitGame();

        public void Resume()
        {
            Hide();
            Time.timeScale = 1f;
        }

        public void Pause()
        {
            Time.timeScale = 0f;
            Show();
        }
    }
}