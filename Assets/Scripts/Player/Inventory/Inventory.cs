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
                    }
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
            UnityEngine.Object.Destroy(player.HeldObject);
            player.HeldObject = null;
        }

        public void EquipedItemUsed(GameObject entity, bool successful)
        {
            GetEquippedItem().OnItemUse(entity, player, successful);
        }

        public Item GetItem(int slot)
        {
            return _itemslots[slot].Item;
        }

        public Item GetEquippedItem()
        {
            return (EquippedItem != null) ? GetItem((int)EquippedItem) : null;
        }

        public ItemSlot GetItemSlot(int slot)
        {
            return _itemslots[slot];
        }

        public List<ItemSlot> GetItemSlots()
        {
            return _itemslots;
        }

        public void AddItem(Item item, bool autoequip = true)
        {
            if (item != null) IsDirty = true;
            else return;
            foreach (ItemSlot slot in _itemslots)
            {
                if (slot.Item == null)
                {
                    slot.Item = item;
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
                    if (slot.Item == item) RemoveItem(slot.Number);
                    break;
                }
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError("Item " + item.Name + " cant be removed from the inventory as it was not in the inventory.");
#endif
            }
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
                AddItem(crafted, false);
                IsDirty = true;
                if (crafted is CraftableItem)
                {
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
