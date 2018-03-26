using System;
using UnityEngine;
using UnityEngine.UI;

public class UIMonoHelper : MonoBehaviour
{
    public static UIMonoHelper Instance;

    public GameObject PauseParent;

    public GameObject InteractableCrosshair;

    public GameObject InventoryParent;
    public Text EquippedItemName;

    public GameObject GrabIcon;
    public GameObject BagIcon;

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

    public GameObject NoteParent;
    public Button NoteNextButton;
    public Button NotePrevButton;
    public Text NoteText;

    public GameObject ItemInfoParent;
    public Text ItemInfoName;
    public Text ItemInfoDescription;
    public Button ViewButton;

    public Text HintText;

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
        EquippedItemName.enabled = false;
        InteractableCrosshair.SetActive(false);
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

    public void NotePrev()
    {
        NoteUI.PreviousPage();
    }

    public void NoteNext()
    {
        NoteUI.NextPage();
    }

    public void NoteView()
    {
        NoteUI.ViewNote();
    }

    public void NoteClose()
    {
        NoteUI.HideNote(this, EventArgs.Empty);
    }
}
