using Items;
using UnityEngine;

namespace Entity
{
    public class SymbolBook : MoveableEntity, IInteractable
    {
        public Symbol SymbolType;

        public bool Interactable
        {
            get { return _interactable; }
            set { _interactable = value; }
        }

        [SerializeField] private bool _interactable = true;

        public void OnInteract(Player player)
        {
            player.inventory.AddItem(new Items.SymbolBook(SymbolType), gameObject, false);
        }
    }
 
}