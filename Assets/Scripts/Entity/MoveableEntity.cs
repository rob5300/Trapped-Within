using UnityEngine;

namespace Entity
{
    [RequireComponent(typeof(Rigidbody))]
    public class MoveableEntity : Entity
    {
        public bool Movable = true;
        public ItemPlaceZone SnapZone;

        public virtual void OnUnsnap()
        {
            SnapZone.Unsnap();
        }
    }
}