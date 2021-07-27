using System;
using Disjointed.Tools.SceneManagement;
using Disjointed.UI;
using UnityEngine;


namespace Disjointed.Game_Management
{
    public class GameManager : ScriptableObject
    {
        public static bool IsPaused => (Time.timeScale == 0f);
        public static bool IsPlaying => (Time.timeScale > 0f);
        
        public static void Pause() => Time.timeScale = 0f;
        public static void Resume() => Time.timeScale = 1f;
        
        public static void Restart()
        {
            SceneSwitcher.ReloadScene();
            Resume();
        }
        
        //> QUIT THE GAME
        public static void Quit() => SceneSwitcher.QuitGame();
    }
}