using System;
using Disjointed.ThePlayer;
using UnityEngine;
using Disjointed.Tools.SceneManagement;


namespace Disjointed.Tools.GameManagement
{
    public class GameManager : ScriptableObject
    {
        public static bool IsPaused => (Time.timeScale == 0f);
        public static bool IsPlaying => (Time.timeScale > 0f);
        
        public static void Pause() => Time.timeScale = 0f;
        public static void Resume() => Time.timeScale = 1f;

        private void Awake()
        {
            Player.playerDeath += Restart;
        }

        public static void Restart()
        {
            SceneSwitcher.ReloadScene();
            Resume();
        }
        
        //> QUIT THE GAME
        public static void Quit() => SceneSwitcher.QuitGame();
    }
}