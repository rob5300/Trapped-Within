using System;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class Item {

        /// <summary>
        /// Returns a new instance of the item type.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Item GetItemInstanceFromType(Type type)
        {
            if (type != null) return (Item)Activator.CreateInstance(type);
            else return null;
        }

        public string Name;
        public string Description;
        public GameObject EntityGameObject;
        public TransformOffset EquipOffset;
        public bool Equipable = true;
        public bool CanDrop = false;

        public Item()
        {
            if (String.IsNullOrEmpty(Name)) Name = GetType().Name;
        }

        public Item(string name)
        {
            Name = name;
        }

        public virtual void OnItemUse(GameObject heldItem, Player player, bool successful)
        {
            
        }
    }

    public struct TransformOffset
    {
        public Vector3 Position;
        public Vector3 RotationEuler;

        public TransformOffset(Vector3 position, Vector3 rotationEuler) 
        {
            Position = position;
            RotationEuler = rotationEuler;
        }
    }
}
