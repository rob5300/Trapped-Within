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
        public OcclusionPortal occlusionPortal;

        protected DoorCloser closer;
        protected Animator animator;

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

        public new void Start()
        {
            animator = GetComponent<Animator>();

            if (!occlusionPortal)
            {
                occlusionPortal = GetComponentInChildren<OcclusionPortal>();
            }

            if(occlusionPortal) occlusionPortal.open = Open;
            closer = gameObject.AddComponent<DoorCloser>();
            closer.door = this;
            closer.enabled = false;
        }

        public void OpenDoor()
        {
            Open = true;
            if(occlusionPortal) occlusionPortal.open = true;
            animator.SetTrigger("Open");
            closer.enabled = true;
        }

        public void CloseDoor()
        {
            Open = false;
            //We need to disable the portal after a delay to ensure that the door is closed visualy before we prevent the gemoetry behind drawing.
            Invoke("DisablePortal", 1.5f);
            animator.SetTrigger("Close");
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

        private void DisablePortal()
        {
            if(!Open && occlusionPortal) occlusionPortal.open = false;
        }
    } 
}
