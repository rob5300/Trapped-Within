using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour {

    public void StartGameButton()
    {
        Game.LoadLevel("Cutscene", "Intro");
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }
}
