using System;
using UnityEngine;
using UnityEngine.UI;

public class UIMonoHelper : MonoBehaviour
{
    public static UIMonoHelper Instance;

    public GameObject PauseParent;

    public GameObject InventoryParent;

    public GameObject TaskLogParent;
    public Text CurrentTaskName;
    public Text CurrentTaskText;

    public Transform CompletedTaskParent;
    public GameObject CompletedTaskPanel;
    public Button TaskLogButton;

    public GameObject FilledItemSlot;
    public GameObject EmptyItemSlot;
    public Transform ItemHolder;
    public Button CraftButton;

    public UIMonoHelper()
    {
        Instance = this;
    }

    public void Start()
    {
        FilledItemSlot.SetActive(false);
        EmptyItemSlot.SetActive(false);
        InventoryParent.SetActive(false);
        CompletedTaskPanel.SetActive(false);
        TaskLogParent.SetActive(false);
        PauseParent.SetActive(false);
    }

    public void Craft()
    {
        Ui.AttemptCraft();
    }

    public void ShowTaskHistory()
    {
        Ui.ShowTaskHistory();
    }

    public void HideTaskHistory()
    {
        Ui.HideTaskHistory(this, EventArgs.Empty);
    }

    public void UnpauseButton()
    {
        Ui.HidePauseMenu();
    }

    public void ToMainMenu()
    {
        Ui.ToMainMenu();
    }
}
