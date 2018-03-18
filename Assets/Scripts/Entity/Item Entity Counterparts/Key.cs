using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Entity
{
    public class Key : MoveableEntity, IInteractable
    {
        public string KeyId;
        [SerializeField]
        private bool interactable;

        public bool Interactable
        {
            get
            {
                return interactable;
            }

            set
            {
                interactable = value;
            }
        }

        public void OnInteract(Player player)
        {
            player.inventory.AddItem(new Items.Key(Name, KeyId), gameObject);
        }
    }
}
