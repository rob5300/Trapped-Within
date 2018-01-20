using System;
using UnityEngine;
using UnityEngine.UI;

public class UIMonoHelper : MonoBehaviour
{
    public static UIMonoHelper Instance;

    public GameObject PauseParent;

    public GameObject InventoryParent;
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
        PauseParent.SetActive(false);
    }

    public void Craft()
    {
        Ui.AttemptCraft();
    }

    public void UnpauseButton()
    {
        Ui.HidePauseMenu();
    }
}
