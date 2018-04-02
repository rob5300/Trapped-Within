using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Entity
{
    [RequireComponent(typeof(Animator))]
    public class Screw : MoveableEntity, IItemInteract
    {
        public Collider InteractCollider;
        public UnityEvent OnUnscrew;

        private Animator _animator;
        private Rigidbody _rigidbody;

        public void Start()
        {
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponentInChildren<Rigidbody>();
        }

        public bool OnItemInteract(Item item, Player player)
        {
            if(item is Items.Screwdriver)
            {
                _animator.SetTrigger("Unscrew");
                Invoke("EnableRigidBody", 1.1f);
                OnUnscrew.Invoke();
                Destroy(InteractCollider);
                return true;
            }
            return false;
        }

        private void EnableRigidBody()
        {
            _rigidbody.isKinematic = false;
            _animator.enabled = false;
            _rigidbody.WakeUp();
            _rigidbody.velocity = new Vector3(0, -0.3f, 0);
        }
    }
}