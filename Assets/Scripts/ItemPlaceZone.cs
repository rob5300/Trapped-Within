using System;
using Entity;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class ItemPlaceZone : MonoBehaviour
{
    public string TypeName;
    public ItemPlaceEvent OnItemEnter;
    public bool DoSnap = false;
    public Transform SnapPosition;

    public void OnTriggerEnter(Collider col)
    {
        if (TypeName == null) return;

        foreach (Entity.Entity entity in col.GetComponents<Entity.Entity>())
        {
            if (TypeName.Equals(entity.GetType().Name))
            {
                //Success, this object has the type we want.
                if(DoSnap) Snap(col, entity);
                break;
            }
        }
    }

    private void Snap(Collider col, Entity.Entity entity)
    {
        Rigidbody rBody = col.GetComponent<Rigidbody>();
        if (rBody) rBody.isKinematic = true;
        if (SnapPosition)
        {
            rBody.transform.position = SnapPosition.position;
            rBody.transform.rotation = SnapPosition.rotation;
        }
        else
        {
            rBody.transform.position = transform.position;
            rBody.transform.rotation = transform.rotation;
        }

        IInteractable interactable = entity as IInteractable;
        if (interactable != null)
        {
            interactable.Interactable = false;
        }

        //MoveableEntity moveEnt = entity as MoveableEntity;
        //if (moveEnt != null)
        //{
        //    
        //}
    }

    public void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }
}

[Serializable]
public class ItemPlaceEvent : UnityEvent<ItemPlaceZone>
{

}
