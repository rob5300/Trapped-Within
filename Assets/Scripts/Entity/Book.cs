﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Entity
{
    public class Book : MoveableEntity, IInteractable
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
        }
    } 
}
