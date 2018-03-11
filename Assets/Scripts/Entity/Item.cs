using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    public class Item : MoveableEntity, IInteractable
    {
        public bool Interactable
        {
            get
            {
                return _interctable;
            }

            set
            {
                _interctable = value;
            }
        }
        [SerializeField]
        bool _interctable = true;

        public virtual void OnInteract(Player player)
        {
            
        }
    } 
}
