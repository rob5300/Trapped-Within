using System;
using UnityEngine;

namespace Entity
{
    [RequireComponent(typeof(Rigidbody))]
    public class MoveableEntity : Entity
    {
        public bool Moveable = true;
        [NonSerialized]
        public ItemPlaceZone SnapZone;

        public virtual void OnUnsnap(bool ignore = true)
        {
            SnapZone.Unsnap(ignore);
        }

        public virtual void OnEntityMoved(Player player)
        {

        }
    }
}