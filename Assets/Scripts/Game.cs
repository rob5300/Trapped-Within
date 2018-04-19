using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For central game data and tasks.
/// </summary>
public static class Game {

    public static Task InitialTask = new Task("How did i get here?", "Make your way out of the room.");
    public static GameObject LoadingScreen { get { return GetLoadingScreen(); } }
    public static LoadingScreen LoadingScreenComponent { get { return GetLoadingScreenComponent(); } }

    public static string CurrentScene = "MainMenu";

    private static GameObject _loadingScreen;
    private static LoadingScreen _loadingScreenComponent;

    public static void Pause()
    {
        Time.timeScale = 0;
    }

    public static void Unpause()
    {
        Time.timeScale = 1;
    }

    public static void LoadLevel(string LevelName, string loadText)
    {
        LoadingScreen.SetActive(true);
        LoadingScreenComponent.LoadingText.text = "Loading:\n" + loadText;
        CurrentScene = LevelName;
        LoadingScreenComponent.LoadScene(LevelName);
    }

    private static GameObject GetLoadingScreen()
    {
        if (!_loadingScreen)
        {
            _loadingScreen = GameObject.FindWithTag("LoadingScreen");
            if (!_loadingScreen) _loadingScreen = Object.Instantiate(Resources.Load<GameObject>("LoadingScreen"));
        }
        return _loadingScreen;
    }

    private static LoadingScreen GetLoadingScreenComponent()
    {
        if(!_loadingScreenComponent) _loadingScreenComponent = GetLoadingScreen().GetComponent<LoadingScreen>();
        return _loadingScreenComponent;
    }
}
