using System;
using UnityEngine;

namespace Entity
{
    [RequireComponent(typeof(Animator))]
    public class Draw : Entity, IInteractable
    {
        public bool Interactable
        {
            get
            {
                return _interactable;
            }

            set
            {
                _interactable = value;
            }
        }
        [SerializeField]
        private bool _interactable = true;
        public bool IsOpen = false;

        public void OnInteract(Player player)
        {
            IsOpen = !IsOpen;
            if (IsOpen)
            {
                Open();
            }
            else
            {
                Close();
            }
        }

        public void Open()
        {
            GetComponent<Animator>().SetTrigger("Open");
        }

        public void Close()
        {
            GetComponent<Animator>().SetTrigger("Close");
        }
    }
}
