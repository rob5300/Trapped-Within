using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Entity
{
    [RequireComponent(typeof(Animator))]
    public class Book : Entity, IInteractable
    {
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
        private Animator _animator;

        public UnityEvent OnActivation;

        public void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void OnInteract(Player player)
        {
            if (!Activated)
            {
                _animator.SetTrigger("Interact");
                Invoke("InvokeEvent", 1);
                Activated = true;
            }
        }

        private void InvokeEvent()
        {
            OnActivation.Invoke();
        }
    } 
}
