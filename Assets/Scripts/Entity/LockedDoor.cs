﻿using System;
using Entity;
using Items;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LockedDoor : Door, IItemInteract
{
    public string KeyId = "";
    public bool Locked = true;

    public override void OnInteract(Player player)
    {
        if (Locked) animator.SetTrigger("DoorLocked");
        else if (InteractOpensDoor)
        {
            ToggleState();
        }
    }

    public void UnlockAndOpen()
    {
        Locked = false;
        OpenDoor();
    }

    public bool OnItemInteract(Items.Item item, Player player)
    {
        if (item is Items.Key)
        {
            Items.Key key = (Items.Key) item;
            if (key.KeyId == KeyId || KeyId == "")
            {
                OpenDoor();
                //Now allow the door to be opened and closed without the key.
                InteractOpensDoor = true;
                Interactable = true;
                Locked = false;
                return true;
            }
        }
        animator.SetTrigger("DoorLocked");
        return false;
    }

    public void Reset()
    {
        InteractOpensDoor = true;
    }
}