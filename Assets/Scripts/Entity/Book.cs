using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Entity
{
    public class Book : Entity, IInteractable
    {

        public string ID = "Book";
        public bool Activated = false;

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
        [SerializeField]
        private bool interactable = true;

        public UnityEvent OnActivation;

        public void OnInteract(Player player)
        {
            if (!Activated)
            {
                OnActivation.Invoke();
                Activated = true;
            }

#if UNITY_EDITOR
            Debug.Log("Book \"" + ID + "\" activated!");
#endif
        }
    } 
}
