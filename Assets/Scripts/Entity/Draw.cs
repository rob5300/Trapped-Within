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
        public bool Open = false;

        public void OnInteract(Player player)
        {
            Open = !Open;
            if (Open)
            {
                GetComponent<Animator>().SetTrigger("Open");
            }
            else
            {
                GetComponent<Animator>().SetTrigger("Close");
            }
        }
    }
}
