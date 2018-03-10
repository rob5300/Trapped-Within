using System;
using Entity;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class ItemPlaceZone : MonoBehaviour
{
    public string TypeName;
    public string PlaceZoneID;
    public ItemPlaceZoneEvent OnItemEnter;
    public ItemPlaceZoneEvent OnItemRemove;
    public MoveableEntity entity;
    private bool DoSnap = true;
    public Transform SnapPosition;
    public bool AllowItemRemoval = true;

    private Collider triggerCollider;
    private MoveableEntity _ignoredEntity;

    public void Start()
    {
        triggerCollider = GetComponent<Collider>() ?? GetComponentInChildren<Collider>();
    }

    public void OnTriggerEnter(Collider col)
    {
        if (!string.IsNullOrEmpty(TypeName))
        {
            foreach (MoveableEntity entity in col.GetComponents<MoveableEntity>())
            {
                if (TypeName.Equals(entity.GetType().Name))
                {
                    //Success, this object has the type we want.
                    if (DoSnap && entity != _ignoredEntity) Snap(col, entity);
                    break;
                }
            }
        }
        else if (PlaceZoneID != null)
        {
            ItemPlaceZoneID id = col.GetComponent<ItemPlaceZoneID>();
            if (id && id.ID == PlaceZoneID)
            {
                //This matches the id. Get the moveable entity and use that for snapping.
                MoveableEntity ent = col.GetComponent<MoveableEntity>();
                if (ent)
                {
                    if (DoSnap && ent != _ignoredEntity) Snap(col, ent);
                }
            }
        }
        
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.GetComponent<MoveableEntity>() == _ignoredEntity) _ignoredEntity = null;
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
        entity.GetComponent<Rigidbody>().isKinematic = false;
        IInteractable interactable = entity as IInteractable;
        if (interactable != null)
        {
            interactable.Interactable = true;
        }

        triggerCollider.enabled = true;
        _ignoredEntity = entity;
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
