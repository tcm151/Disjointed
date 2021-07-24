using System;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;


namespace Disjointed.Tools.Editor
{
    public class BuildWindow : EditorWindow
    {
        private static string version;
        
        private static BuildWindow window;
        
        [MenuItem("Build/Build Window")]
        private static void ShowWindow()
        {
            window = GetWindow<BuildWindow>();
            window.titleContent = new GUIContent("Build Window");
            window.Show();

            
        }

        private void OnEnable()
        {
            version = PlayerSettings.bundleVersion;
        }

        private void OnGUI()
        {
            GUILayout.Label("Build Settings", EditorStyles.boldLabel);
            GUILayout.Space(4);
            version = EditorGUILayout.TextField("Version", version);
            
            GUILayout.Space(4);
            if (GUILayout.Button("Build Game"))
            {
                BuildGame();
            }
        }

        private void BuildGame()
        {
            var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = new [] {"Assets/Scenes/The Cave.unity"},
                locationPathName = $"Builds/{version}/Disjointed.exe",
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.None,
            });

            if (report.summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"Build succeeded: {report.summary.totalSize/1e6} MB");
                PlayerSettings.bundleVersion = version;
            }

            if (report.summary.result == BuildResult.Failed)
            {
                Debug.Log("Build failed!");
            }
        }
    }
}