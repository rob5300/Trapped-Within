using System;
using System.Collections.Generic;
using Items;
using UnityEngine;
using Object = UnityEngine.Object;

public static class Ui
{
    public static bool InventoryVisible = false;
    public static bool PauseMenuVisible = false;

    private static List<GameObject> _slots = new List<GameObject>();
    private static List<int> _selectedSlots = new List<int>();
    public static List<EventHandler<EventArgs>> EscapeEvents = new List<EventHandler<EventArgs>>();

    //EventHandlers
    private static readonly EventHandler<EventArgs> _inventoryEventHandle = HideInventory;

    public static void Escape()
    {
        if (EscapeEvents.Count == 0)
        {
            TogglePauseMenu();
        }
        else
        {
            EscapeEvents[EscapeEvents.Count - 1].Invoke(null, null);
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
                _slots.Add(newSlot);
            }
            //Slot is empty
            else
            {
                GameObject newSlot = Object.Instantiate(UIMonoHelper.Instance.EmptyItemSlot, UIMonoHelper.Instance.ItemHolder);
                newSlot.name = slot.Number.ToString();
                newSlot.SetActive(true);
                _slots.Add(newSlot);
            }
        }

        Player.instance.inventory.IsDirty = false;
    }

    public static void CleanInventory()
    {
        //Destroy all slots.
        for (int slotno = _slots.Count - 1; slotno > -1; slotno--)
        {
            Object.Destroy(_slots[slotno]);
        }
        _slots = new List<GameObject>();
    }

    public static void ToggleInventory()
    {
        if(InventoryVisible) HideInventory(null, null);
        else if(!PauseMenuVisible) ShowInventory();
    }

    public static void ShowInventory()
    {
        if (Player.instance.inventory.IsDirty)
        {
            CleanInventory();
            PopulateInventory();
        }
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
        foreach (GameObject uislot in _slots)
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
                foreach (GameObject uislot in _slots)
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
}
