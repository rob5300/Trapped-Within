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
        public bool Locked = false;

        private Animator animator;
        private AudioSource audio;

        public void Start()
        {
            animator = GetComponent<Animator>();
            audio = GetComponentInParent<AudioSource>();
        }

        public void OnInteract(Player player)
        {
            if (!Locked)
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
            else
            {
                animator.SetTrigger("Locked");
                if (!audio.isPlaying) audio.Play();
            }
        }

        public void Open()
        {
            animator.SetTrigger("Open");
        }

        public void Close()
        {
            animator.SetTrigger("Close");
        }
    }
}
