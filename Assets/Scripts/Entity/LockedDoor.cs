using System;
using Entity;
using Items;
using UnityEngine;

public class LockedDoor : Door, IItemInteract
{
    public string KeyId = "";

    public bool OnItemInteract(Items.Item item, Player player)
    {
        if (item is Items.Key)
        {
            Items.Key key = (Items.Key) item;
            if (key.KeyId == KeyId || KeyId == "")
            {
                OpenDoor();
                return true;
            }
        }
        return false;
    }
}