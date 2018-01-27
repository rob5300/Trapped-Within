using System;
using System.Collections.Generic;
using Items;
using UnityEngine;
using Object = UnityEngine.Object;

public static class Ui
{
    public static bool InventoryVisible = false;
    public static bool PauseMenuVisible = false;

    private static List<GameObject> _inventorySlots = new List<GameObject>();
    private static List<int> _selectedSlots = new List<int>();
    private static List<GameObject> _pastTaskPanels = new List<GameObject>();
    private static Char _speechMarks = '"';

    public static List<EventHandler<EventArgs>> EscapeEvents = new List<EventHandler<EventArgs>>();
    public static List<EventHandler<EventArgs>> ExtraWindowEvents = new List<EventHandler<EventArgs>>();

    //EventHandlers for EscapeEvents
    private static readonly EventHandler<EventArgs> _inventoryEventHandle = HideInventory;
    private static readonly EventHandler<EventArgs> _taskLogEventHandle = HideTaskHistory;

    public static void Escape()
    {
        if (EscapeEvents.Count == 0)
        {
            TogglePauseMenu();
        }
        else
        {
            EscapeEvents[EscapeEvents.Count - 1].Invoke(null, EventArgs.Empty);
        }
    }

    public static void TogglePauseMenu()
    {
        if (PauseMenuVisible)
        {
            HidePauseMenu();
        }
        else
        {
            ShowPauseMenu();
        }
    }

    public static void ShowPauseMenu()
    {
        UIMonoHelper.Instance.PauseParent.SetActive(true);
        PauseMenuVisible = true;
        ShowCursor();
        Player.instance.movement.DoMovement = false;
        Player.instance.movement.DoRotation = false;

        Game.Pause();
    }

    public static void HidePauseMenu()
    {
        UIMonoHelper.Instance.PauseParent.SetActive(false);
        PauseMenuVisible = false;
        HideCursor();
        Player.instance.movement.DoMovement = true;
        Player.instance.movement.DoRotation = true;

        Game.Unpause();
    }

    public static void PopulateInventory()
    {
        List<ItemSlot> itemslots = Player.instance.inventory.GetItemSlots();
        //Set the capacity to prevent constant resizing.
        _inventorySlots.Capacity = itemslots.Count;
        foreach (ItemSlot slot in itemslots)
        {
            //Item slot is not empty
            if (slot.Item != null)
            {
                GameObject newSlot = Object.Instantiate(UIMonoHelper.Instance.FilledItemSlot, UIMonoHelper.Instance.ItemHolder);
                newSlot.GetComponent<UIFilledSlot>().Slot = slot.Number;
                newSlot.GetComponent<UIFilledSlot>().Name.text = slot.Item.Name;

                if (slot.Number == Player.instance.inventory.EquippedItem)
                    newSlot.GetComponent<UIFilledSlot>().EquipButton.interactable = false;

                newSlot.SetActive(true);
                _inventorySlots.Add(newSlot);
                //Remove the object to save memory, its no longer needed.
                Object.Destroy(newSlot.GetComponent<UIFilledSlot>());
            }
            //Slot is empty
            else
            {
                GameObject newSlot = Object.Instantiate(UIMonoHelper.Instance.EmptyItemSlot, UIMonoHelper.Instance.ItemHolder);
                newSlot.name = slot.Number.ToString();
                newSlot.SetActive(true);
                _inventorySlots.Add(newSlot);
            }
        }

        Player.instance.inventory.IsDirty = false;
    }

    public static void CleanInventory()
    {
        //Destroy all slots.
        for (int slotno = _inventorySlots.Count - 1; slotno > -1; slotno--)
        {
            Object.Destroy(_inventorySlots[slotno]);
        }
        _inventorySlots.Clear();
    }

    public static void ToggleInventory()
    {
        if(InventoryVisible) HideInventory(null, null);
        else if(!PauseMenuVisible) ShowInventory();
    }

    public static void ShowInventory()
    {
        //Update depending on dirty states
        if (Player.instance.inventory.IsDirty)
        {
            CleanInventory();
            PopulateInventory();
        }
        if (Player.instance.TaskManager.CurrentIsDirty)
        {
            UpdateCurrenTask();
        }

        //Check if we have any tasks in the history. If soo we enable the button. This is done in this manor to allow for save states in future.
        UIMonoHelper.Instance.TaskLogButton.interactable = Player.instance.TaskManager.PastTasks.Count != 0;

        UIMonoHelper.Instance.InventoryParent.SetActive(true);
        InventoryVisible = true;
        ShowCursor();
        Player.instance.movement.DoMovement = false;
        Player.instance.movement.DoRotation = false;

        //Add to escape event
        EscapeEvents.Add(_inventoryEventHandle);
    }

    public static void HideInventory(object ob, EventArgs e)
    {
        UIMonoHelper.Instance.InventoryParent.SetActive(false);
        _selectedSlots = new List<int>();
        foreach (GameObject uislot in _inventorySlots)
        {
            UIFilledSlot fslot = uislot.GetComponent<UIFilledSlot>();
            if (fslot != null)
            {
                fslot.SelectToggle.isOn = false;
            }
        }
        UIMonoHelper.Instance.CraftButton.interactable = false;
        InventoryVisible = false;
        HideCursor();
        Player.instance.movement.DoMovement = true;
        Player.instance.movement.DoRotation = true;

        if (EscapeEvents.Contains(_inventoryEventHandle)) EscapeEvents.Remove(_inventoryEventHandle);
        HideAllWindows();
    }

    public static void ShowTaskHistory()
    {
        UIMonoHelper.Instance.TaskLogParent.SetActive(true);
        EscapeEvents.Add(_taskLogEventHandle);
        ExtraWindowEvents.Add(_taskLogEventHandle);
        if (Player.instance.TaskManager.PastIsDirty)
        {
            GameObject newTaskPanel;
            UIPastTaskPanel newTaskPanelComponent;
            Task pastTask;

            //Set capacity to prevent resizing. Optimisation!
            _pastTaskPanels.Capacity = Player.instance.TaskManager.PastTasks.Count;

            for(int i = _pastTaskPanels.Count; i < Player.instance.TaskManager.PastTasks.Count; i++)
            {
                pastTask = Player.instance.TaskManager.PastTasks[i];
                newTaskPanel = Object.Instantiate(UIMonoHelper.Instance.CompletedTaskPanel, UIMonoHelper.Instance.CompletedTaskParent);
                newTaskPanelComponent = newTaskPanel.GetComponent<UIPastTaskPanel>();
                newTaskPanelComponent.TaskTitle.text = _speechMarks + pastTask.Name + _speechMarks;
                newTaskPanelComponent.TaskDescription.text = pastTask.Description;
                newTaskPanel.SetActive(true);
                _pastTaskPanels.Add(newTaskPanel);
                Object.Destroy(newTaskPanelComponent); 
            }
            Player.instance.TaskManager.PastIsDirty = false;
        }
    }

    public static void HideTaskHistory(object sender, EventArgs args)
    {
        UIMonoHelper.Instance.TaskLogParent.SetActive(false);
        if (EscapeEvents.Contains(_taskLogEventHandle)) EscapeEvents.Remove(_taskLogEventHandle);
        if (ExtraWindowEvents.Contains(_taskLogEventHandle)) ExtraWindowEvents.Remove(_taskLogEventHandle);
    }

    private static void UpdateCurrenTask()
    {
        UIMonoHelper.Instance.CurrentTaskText.text = Player.instance.TaskManager.CurrentTask.Description;
        UIMonoHelper.Instance.CurrentTaskName.text = _speechMarks + Player.instance.TaskManager.CurrentTask.Name + _speechMarks;
        Player.instance.TaskManager.CurrentIsDirty = false;
    }

    public static void AttemptCraft()
    {
        if (_selectedSlots.Count != 0)
        {
            if (Player.instance.inventory.AttemptCraft(_selectedSlots.ToArray()))
            {

                CleanInventory();
                PopulateInventory();
            }
            else
            {
                foreach (GameObject uislot in _inventorySlots)
                {
                    UIFilledSlot fslot = uislot.GetComponent<UIFilledSlot>();
                    if (fslot != null)
                    {
                        fslot.SelectToggle.isOn = false;
                    }
                }
                _selectedSlots = new List<int>();
            }
        }
    }

    public static void UiSlotToggle(int slotNumber, bool state)
    {
        if (state) _selectedSlots.Add(slotNumber);
        else _selectedSlots.Remove(slotNumber);

        UIMonoHelper.Instance.CraftButton.interactable = _selectedSlots.Count > 0;
    }

    public static void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public static void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static void EquipItem(int slot)
    {
        Player.instance.inventory.EquipItem(slot);
    }

    private static void HideAllWindows()
    {
        foreach (EventHandler<EventArgs> window in ExtraWindowEvents)
        {
            window.Invoke(null, EventArgs.Empty);
        }
        ExtraWindowEvents.Clear();
    }
}
