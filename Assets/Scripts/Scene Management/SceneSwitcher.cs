using UnityEngine;
using UnityEngine.SceneManagement;

namespace Disjointed.SceneManagement
{
    public static class SceneSwitcher
    {
        public static void LoadScene(int newScene) => SceneManager.LoadScene(newScene);

        public static void QuitGame()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
            
        }

        public static void ReloadScene()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.buildIndex);
        }
    }
}
