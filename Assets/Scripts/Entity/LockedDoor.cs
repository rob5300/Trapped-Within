using System;
using Entity;
using Items;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LockedDoor : Door, IItemInteract
{
    public string KeyId = "";

    private Animator animator;

    public new void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void OnInteract(Player player)
    {
        if(!Open) animator.SetTrigger("DoorLocked");
    }

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
        animator.SetTrigger("DoorLocked");
        return false;
    }
}