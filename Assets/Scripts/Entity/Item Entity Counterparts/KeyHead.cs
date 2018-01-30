using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Entity
{
    public class KeyHead : MoveableEntity, IInteractable
    {
        public bool Interactable {
            get { return _interactable; }

            set { _interactable = value; }
        }
        [SerializeField]
        private bool _interactable = true;

        public string CraftedKeyId = "Default";

        public void OnInteract(Player player)
        {
            player.inventory.AddItem(new Items.KeyHead(CraftedKeyId), gameObject);
        }
    }
}
