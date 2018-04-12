using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    public class BoltedCover : MoveableEntity
    {
        public int AttachedScrews = 0;
        private Rigidbody rigidbody;

        public void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        public override void OnEntityMoved(Player player)
        {
            if (rigidbody.isKinematic && AttachedScrews < 1) rigidbody.isKinematic = false;
        }

        public void RemoveScrew()
        {
            AttachedScrews--;
            if (AttachedScrews < 1) Moveable = true;
        }
    }
}
