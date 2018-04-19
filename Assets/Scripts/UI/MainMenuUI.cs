using UnityEngine;
using UnityEngine.PostProcessing;

public class MainMenuUI : MonoBehaviour {

    PostProcessingProfile profile;

    public void Start()
    {
        Game.Unpause();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SaveConfig()
    {
        Configuration.SaveConfig();
    }

    public void StartGameButton()
    {
        Game.LoadLevel("Cutscene", "Intro");
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }
}
