using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    [RequireComponent(typeof(Animator))]
    public class Rug : Entity, IInteractable
    {
        public bool Interactable {
            get { return _interactable; }
            set { _interactable = value; }
        }
        [SerializeField]
        private bool _interactable = true;

        public GameObject HiddenObject;
        public bool PulledUp = false;

        private Collider col;
        private Animator anim;

        public void Start()
        {
            if(HiddenObject.activeSelf) HiddenObject.SetActive(false);
            col = GetComponent<Collider>();
            anim = GetComponent<Animator>();
            //Ignore collisions between the rug and its hidden object.
            Collider hiddenObCollider =
                HiddenObject.GetComponent<Collider>();
            if(!hiddenObCollider) hiddenObCollider = HiddenObject.GetComponentInChildren<Collider>();
            Physics.IgnoreCollision(col, hiddenObCollider);
        }

        public void OnInteract(Player player)
        {
            if (HiddenObject && !PulledUp)
            {
                anim.SetTrigger("Pull Up");
                PulledUp = true;
                HiddenObject.SetActive(true);
                //Make the rug now ignore raycasts to let the hidden object be interacted with and touched;
                gameObject.layer = 2;
            }
        }
    }

}