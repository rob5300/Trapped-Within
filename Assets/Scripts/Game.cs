using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For central game data and tasks.
/// </summary>
public static class Game {

    public static Task InitialTask = new Task("How did i get here?", "Find a way to escape the room.");

    public static void Pause()
    {
        Time.timeScale = 0;
    }

    public static void Unpause()
    {
        Time.timeScale = 1;
    }

    
}
