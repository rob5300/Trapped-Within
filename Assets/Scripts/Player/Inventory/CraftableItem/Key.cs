using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class Key : CraftableItem
    {
        /// <summary>
        /// The id of this key.
        /// </summary>
        public string KeyId = "default";

        public Key()
        {
            Name = "Key";
            EntityGameObject = Resources.Load<GameObject>("CraftableItems/Key");
        }

        public Key(string name) : this()
        {
            Name = name;
        }

        public Key(string name, string keyId) : this(name)
        {
            KeyId = KeyId.ToLower();
        }

        public override void OnItemCrafted(List<CraftingComponent> craftingComponents)
        {
            foreach (CraftingComponent component in craftingComponents)
            {
                if (component is KeyHead)
                {
                    KeyId = ((KeyHead) component).CraftedKeyId;
                }
            }
        }

        public override void OnItemUse(GameObject heldItem, Player player, bool successful)
        {
            if(successful) player.inventory.RemoveItem(this);
        }

        public override void AssignData(Entity.Entity key)
        {
            ((Entity.Key)key).KeyId = KeyId;
        }
    }
}
