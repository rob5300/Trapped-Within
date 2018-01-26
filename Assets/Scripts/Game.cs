﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Game {

    public static Task InitialTask = new Task("01", "Find a way to escape the room.");

    public static void Pause()
    {
        Time.timeScale = 0;
    }

    public static void Unpause()
    {
        Time.timeScale = 1;
    }

    
}
