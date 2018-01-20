using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour {

    public void StartGameButton()
    {
        SceneManager.LoadScene("Start");
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }
}
