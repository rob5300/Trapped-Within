using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    public class Screwdriver : MoveableEntity, IInteractable
    {
        [SerializeField]
        private bool interactable = true;

        public bool Interactable {
            get {
                return interactable;
            }

            set {
                interactable = value;
            }
        }

        public void OnInteract(Player player)
        {
            player.inventory.AddItem(new Items.Screwdriver(), gameObject);
        }
    }
}