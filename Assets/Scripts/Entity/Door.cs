using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    [RequireComponent(typeof(Animator))]
    public class Door : Entity, IInteractable
    {
        public bool Open = false;
        public bool InteractOpensDoor = false;

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
        private bool interactable = false;

        public void OpenDoor()
        {
            Open = true;
            GetComponent<Animator>().SetTrigger("Open");
        }

        public void CloseDoor()
        {
            Open = false;
            GetComponent<Animator>().SetTrigger("Close");
        }

        public bool ToggleState()
        {
            if (Open)
            {
                CloseDoor();
            }
            else
            {
                OpenDoor();
            }
            return Open;
        }

        public virtual void OnInteract(Player player)
        {
            if (InteractOpensDoor)
            {
                ToggleState();
            }
        }
    } 
}
