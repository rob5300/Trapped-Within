using System;
using Entity;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class ItemPlaceZone : MonoBehaviour
{
    public string TypeName;
    public ItemPlaceZoneEvent OnItemEnter;
    public ItemPlaceZoneEvent OnItemRemove;
    public MoveableEntity entity;
    private bool DoSnap = true;
    public Transform SnapPosition;
    public bool AllowItemRemoval = true;

    private Collider triggerCollider;

    public void Start()
    {
        triggerCollider = GetComponent<Collider>() ?? GetComponentInChildren<Collider>();
    }

    public void OnTriggerEnter(Collider col)
    {
        if (TypeName == null) return;

        foreach (MoveableEntity entity in col.GetComponents<MoveableEntity>())
        {
            if (TypeName.Equals(entity.GetType().Name))
            {
                //Success, this object has the type we want.
                if(DoSnap) Snap(col, entity);
                break;
            }
        }
    }

    private void Snap(Collider col, MoveableEntity ent)
    {
        entity = ent;
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

        if (!AllowItemRemoval)
        {
            entity.Movable = false;
        }
        else
        {
            entity.SnapZone = this;
        }
        DoSnap = false;
        triggerCollider.enabled = false;
        OnItemEnter.Invoke(this, entity);
    }

    public void Unsnap()
    {
        entity.GetComponent<Rigidbody>().isKinematic = true;
        IInteractable interactable = entity as IInteractable;
        if (interactable != null)
        {
            interactable.Interactable = true;
        }

        triggerCollider.enabled = true;
        entity.SnapZone = null;
        entity = null;
        OnItemRemove.Invoke(this, entity);
        DoSnap = true;
    }

    public void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }
}

[Serializable]
public class ItemPlaceZoneEvent : UnityEvent<ItemPlaceZone, MoveableEntity>
{

}
