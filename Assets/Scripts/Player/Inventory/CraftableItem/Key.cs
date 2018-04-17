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
        public new string Name = "Key";

        public Key()
        {
            EntityGameObject = Resources.Load<GameObject>("CraftableItems/Key");
            CanDrop = true;
            EquipOffset = new TransformOffset(new Vector3(0, 0, -0.03f), new Vector3(0, 180, 0));
        }

        public Key(string name) : this()
        {
            Name = name;
        }

        public Key(string name, string keyId) : this(name)
        {
            KeyId = keyId.ToLower();
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
