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

        public Collider CollisionCollider;
        private Collider hiddenObCollider;
        private Animator anim;

        public void Start()
        {
            if(HiddenObject.activeSelf) HiddenObject.SetActive(false);
            anim = GetComponent<Animator>();
            //Ignore collisions between the rug and its hidden object.
            hiddenObCollider =
                HiddenObject.GetComponent<Collider>();
            if(!hiddenObCollider) hiddenObCollider = HiddenObject.GetComponentInChildren<Collider>();
            Physics.IgnoreCollision(CollisionCollider, hiddenObCollider);
        }

        public void OnTriggerExit(Collider col)
        {
            if (PulledUp && col.gameObject == HiddenObject)
            {
                Physics.IgnoreCollision(CollisionCollider, hiddenObCollider, false);
            }
            else if(col.gameObject.transform.parent.gameObject == HiddenObject)
            {
                Physics.IgnoreCollision(CollisionCollider, hiddenObCollider, false);
            }
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