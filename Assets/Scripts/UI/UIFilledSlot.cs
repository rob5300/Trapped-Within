using System;
using UnityEngine;
using UnityEngine.UI;

public class UIFilledSlot : MonoBehaviour
{
    public Text Name;
    public Image Icon;
    public Toggle SelectToggle;
    public int Slot;
    public Button EquipButton;
    public Button DropButton;

    public void OnToggle(bool toggleVal)
    {
        Ui.UiSlotToggle(Slot, toggleVal);
    }

    public void EquipItem()
    {
        Ui.EquipItem(Slot);
        EquipButton.interactable = false;
    }

    public void DropItem()
    {
        //Drop this slots item, then destroy the slot as the item was dropped.
        Player.instance.inventory.DropItem(Slot, Player.instance.GetDropPosition());
        Destroy(gameObject);
    }
}
