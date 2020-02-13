using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class AvailableScenes
{
    public const string
        GalaxyMap = "GalaxyMap",
        MainMenu = "MainMenu",
        PauseMenu = "PauseMenu",
        Encounter = "Encounter";
}

public class SceneController : MonoBehaviour
{
    public static void LoadScene(string sceneName)
    {
        Dictionary<string, int> scenes = new Dictionary<string, int>();
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        int sceneIndex;

        // Generate dictionary of scenes from the scenes in Unity's build settings (less error prone)
        for (int i = 0; i < sceneCount; i++)
        {
            scenes.Add(System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i)), i);
        }

        sceneIndex = scenes[sceneName];
        SceneManager.LoadScene(sceneIndex);
    }
}