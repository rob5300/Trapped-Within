using Items;
using UnityEngine;

public class ItemInteractSwitch : MonoBehaviour, IItemInteract
{
    public bool HasInteracted = false;
    public GameObject[] ToEnable;
    public ItemInteractEvent InteractEvent;
    public string TypeAllowedName = "Item";

    public bool OnItemInteract(Item item, Player player)
    {
        if (item.GetType().Name == TypeAllowedName && !HasInteracted)
        {
            if (ToEnable.Length != 0)
            {
                foreach (GameObject g in ToEnable)
                {
                    g.SetActive(true);
                }
            }
            InteractEvent.Invoke(item);
            return true;
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogWarning("ItemInteractSwitch wasn't activated as used item type " + item.GetType().Name + " doesn't match requested type " + TypeAllowedName);
#endif
            return false;
        }
    }
}
