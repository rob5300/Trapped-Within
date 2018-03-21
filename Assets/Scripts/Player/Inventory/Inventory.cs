using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Items
{
    public class Inventory
    {
        public int Capacity { get; private set; }
        public bool IsDirty;
        public int? EquippedItem;
        private int EquippedItemsLayer = 0;
        public Player player;

        private List<ItemSlot> _itemslots;

        public Inventory(int capacity, Player player)
        {
            _itemslots = new List<ItemSlot>();
            Capacity = capacity - 1;
            IsDirty = true;
            this.player = player;

            //Populate the itemslots list with empty slots.
            for (int i = 0; i < Capacity; i++)
            {
                _itemslots.Add(new ItemSlot(i));
            }
        }

        public void EquipItem(int slot)
        {
            if (_itemslots[slot].Item != null)
            {
                if (_itemslots[slot].Item.Equipable && _itemslots[slot].Item.EntityGameObject != null)
                {
                    EquippedItem = slot;
                    //If this is a prefab then we instantiate it.
                    if (!_itemslots[slot].Item.EntityGameObject.scene.isLoaded)
                    {
                        GameObject newob = Object.Instantiate<GameObject>(_itemslots[slot].Item.EntityGameObject,
                            player.HoldPoint.position, player.HoldPoint.rotation,
                            player.HoldPoint);
                        Rigidbody rb = newob.GetComponent<Rigidbody>();
                        if (rb)
                        {
                            rb.isKinematic = true;
                        }
                        player.HeldObject = newob;
                    }
                    else
                    {
                        _itemslots[slot].Item.EntityGameObject.transform.parent = player.HoldPoint;
                        _itemslots[slot].Item.EntityGameObject.transform.localPosition = Vector3.zero;
                        Rigidbody rb = _itemslots[slot].Item.EntityGameObject.GetComponent<Rigidbody>();
                        if (rb)
                        {
                            rb.isKinematic = true;
                        }
                        player.HeldObject = _itemslots[slot].Item.EntityGameObject;
                        player.HeldObject.SetActive(true);
                    }
                    //Change the items layer to prevent it from being raycast
                    EquippedItemsLayer = _itemslots[slot].Item.EntityGameObject.layer;
                    _itemslots[slot].Item.EntityGameObject.layer = 2;

                    //Show the name of the item on the HUD
                    UIMonoHelper.Instance.EquippedItemName.text = GetEquippedItem().Name;
                    UIMonoHelper.Instance.EquippedItemName.enabled = true;
                }
#if UNITY_EDITOR
                else if (_itemslots[slot].Item.Equipable && _itemslots[slot].Item.EntityGameObject == null)
                {
                    Debug.LogError("Item at slot " + slot + " cannot be equipped as it's Entity Gameobject is null.");
                }
#endif
            }
#if UNITY_EDITOR
            else Debug.LogError("Item at slot " + slot + " cannot be equipped as it's null.");
#endif
        }

        public void UnequipItem()
        {
            EquippedItem = null;
            player.HeldObject.transform.parent = null;
            player.HeldObject.SetActive(false);
            player.HeldObject.layer = EquippedItemsLayer;
            EquippedItemsLayer = 0;
            player.HeldObject = null;

            UIMonoHelper.Instance.EquippedItemName.enabled = false;
        }

        public void EquipedItemUsed(GameObject entity, bool successful)
        {
            Item eItem = GetEquippedItem();
            //We check if its actually null, because Snap zones will make the item drop before this can finish. Soo it was not used properly.
            if(eItem != null) eItem.OnItemUse(entity, player, successful);
        }

        public Item GetItem(int slot)
        {
            return _itemslots[slot].Item;
        }

        public Item GetEquippedItem()
        {
            if (EquippedItem.HasValue)
            {
                return GetItem(EquippedItem.Value);
            }
            return null;
        }

        public ItemSlot GetItemSlot(int slot)
        {
            return _itemslots[slot];
        }

        public List<ItemSlot> GetItemSlots()
        {
            return _itemslots;
        }

        public void AddItem(Item item, GameObject sourceGameObject, bool autoequip = true)
        {
            if (item != null) IsDirty = true;
            else return;
            foreach (ItemSlot slot in _itemslots)
            {
                if (slot.Item == null)
                {
                    slot.Item = item;
                    //We set the gameobject to be the one provided instead.
                    //We then disable it.
                    if (sourceGameObject)
                    {
                        slot.Item.EntityGameObject = sourceGameObject;
                        sourceGameObject.SetActive(false);
                    }
                    if (EquippedItem == null && autoequip && item.Equipable)
                    {
                        EquipItem(slot.Number);
                    }
                    break;
                }
            }
        }

        public void RemoveItem(int slot)
        {
            _itemslots[slot].Item = null;
            if (slot == EquippedItem)
            {
                //If the item was equipped, unequip it.
                UnequipItem();
            }
            IsDirty = true;
        }

        public void RemoveItem(Item item)
        {
            List<ItemSlot> returns = _itemslots.Where(x => x.Item == item).ToList();
            if (returns.Count > 0)
            {
                foreach (ItemSlot slot in _itemslots)
                {
                    if (slot.Item == item)
                    {
                        RemoveItem(slot.Number);
                        break;
                    }
                }
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError("Item " + item.Name + " cant be removed from the inventory as it was not in the inventory.");
#endif
            }
        }

        public GameObject DropItem(int slot, Vector3 position)
        {
            return DropItem(GetItem(slot), position);
        }

        public GameObject DropItem(Item item, Vector3 position)
        {
            //Assign the data from the new item to its entity
            //((CraftableItem)crafted).AssignData(((CraftableItem)crafted).EntityGameObject.GetComponent<Entity.Entity>());
            if (!item.EntityGameObject && item.CanDrop) return null;
            if (!item.EntityGameObject.scene.isLoaded)
            {
                //This is an unloaded gameobject, that is in resources, instantiate it first.
                item.EntityGameObject = Object.Instantiate(item.EntityGameObject);
                //Assign the data from the item to its new gameobject if this is a craftable item.
                if (item is CraftableItem)
                {
                    ((CraftableItem)item).AssignData(item.EntityGameObject.GetComponent<Entity.Entity>());
                }
            }
            //Move the objects position. Remove its old parent.
            item.EntityGameObject.transform.parent = null;
            item.EntityGameObject.transform.position = position;
            item.EntityGameObject.GetComponent<Rigidbody>().isKinematic = false;
            //Remove it from the inventory.
            RemoveItem(item);
            //Enable it last, as if the item was equipped it may have been de activated.
            item.EntityGameObject.SetActive(true);
            return item.EntityGameObject;
        }

        public ItemSlot[] GetPopulatedItemSlots()
        {
            ItemSlot[] slots = _itemslots.Where(x => x.Item != null).ToArray();
            if (slots.Length != 0) return slots;
            else return null;
        }

        /// <summary>
        /// Attempt to craft an item with the provided item slots. Returns true if the craft was successful.
        /// </summary>
        /// <param name="slots">Slots of the items to use in the craft.</param>
        /// <returns>If the craft was successful</returns>
        public bool AttemptCraft(params int[] slots)
        {
            //Get a list of types from the slots provided.
            List<Type> mats = new List<Type>();
            foreach (int slotno in slots)
            {
                Item i = GetItem(slotno);
                if (i is CraftingComponent)
                {
                    mats.Add(i.GetType());
                }
            }
            //Attempt to get a new item instance from the given material types.
            Item crafted = null;
            if (mats.Count != 0) crafted = CraftableItem.GetItemInstanceFromCraftingComponents(mats.ToArray());

            if (crafted != null)
            {
                //An item was crafted, save the crafting component instances into a list.
                List<CraftingComponent> componentItems = new List<CraftingComponent>();
                for (int i = 0; i < slots.Length; i++)
                {
                    componentItems.Add((CraftingComponent)_itemslots[slots[i]].Item);
                }
                //Remove the crafting materials from the inventory
                foreach (int slot in slots)
                {
                    RemoveItem(slot);
                }

                //Add the new item to the inventory. This is done after the old materials are removed to ensure the new item takes the lowest slot.
                AddItem(crafted, null, false);
                IsDirty = true;
                if (crafted is CraftableItem)
                {
                    //Tell the item that it was crafted and pass the used items.
                    ((CraftableItem)crafted).OnItemCrafted(componentItems);
                }
#if UNITY_EDITOR
                else Debug.LogError("Crafted item was not of type CraftableItem! Item was: " + crafted.Name + ".");
#endif
                return true;
            }
            return false;
        }
    }
}
