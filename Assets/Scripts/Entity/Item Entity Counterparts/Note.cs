using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    public class Note : Entity, IInteractable
    {
        public bool Interactable {
            get { return _interactable; }

            set { _interactable = value; }
        }
        [SerializeField]
        private bool _interactable = true;

        public string[] Contents;

        public void OnInteract(Player player)
        {
            player.inventory.AddItem(new Items.Note(Name, Description, Contents), null, false);
            Destroy(gameObject);
        }
    }
}
