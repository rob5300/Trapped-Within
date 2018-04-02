using System;
using Entity;
using Items;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class ItemPlaceZone : MonoBehaviour, IItemInteract
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
    [NonSerialized]
    private MoveableEntity _ignoredEntity;

    public void Start()
    {
        triggerCollider = GetComponent<Collider>() ?? GetComponentInChildren<Collider>();
    }

    public void OnTriggerEnter(Collider col)
    {
        CheckIfValid(col.gameObject);
    }

    private void CheckIfValid(GameObject gObject)
    {
        //For trigger entry game objects.
        if (!string.IsNullOrEmpty(TypeName))
        {
            foreach (MoveableEntity ent in gObject.GetComponents<MoveableEntity>())
            {
                if (TypeName.Equals(ent.GetType().Name))
                {
                    //Success, this object has the type we want.
                    if (DoSnap && ent != _ignoredEntity &&
                        (Player.instance.inventory.EquippedItem != null ? gObject != Player.instance.inventory.GetEquippedItem().EntityGameObject : true))
                    {
                        Snap(gObject, ent);
                    }
                    break;
                }
            }
        }
        else if (PlaceZoneID != null)
        {
            ItemPlaceZoneID id = gObject.GetComponent<ItemPlaceZoneID>();
            if (id && id.ID == PlaceZoneID)
            {
                //This matches the id. Get the moveable entity and use that for snapping.
                MoveableEntity ent = gObject.GetComponent<MoveableEntity>();
                if (ent)
                {
                    if (DoSnap && ent != _ignoredEntity) Snap(gObject, ent);
                }
            }
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.GetComponent<MoveableEntity>() == _ignoredEntity) _ignoredEntity = null;
    }

    private void Snap(GameObject gObject, MoveableEntity ent)
    {
        entity = ent;
        Rigidbody rBody = gObject.GetComponent<Rigidbody>();
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
        if (!AllowItemRemoval)
        {
            entity.Moveable = false;
            if (interactable != null) interactable.Interactable = false;
        }
        else
        {
            entity.SnapZone = this;
        }
        DoSnap = false;
        triggerCollider.enabled = false;
        OnItemEnter.Invoke(this, entity);
    }

    public void Unsnap(bool ignore)
    {
        entity.GetComponent<Rigidbody>().isKinematic = false;
        IInteractable interactable = entity as IInteractable;
        if (interactable != null)
        {
            interactable.Interactable = true;
        }

        triggerCollider.enabled = true;
        if (ignore) _ignoredEntity = entity;
        entity.SnapZone = null;
        entity = null;
        OnItemRemove.Invoke(this, entity);
        DoSnap = true;
    }

    public void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    public bool OnItemInteract(Items.Item item, Player player)
    {
        return CheckIfValidItem(item.EntityGameObject, item, player);
    }

    private bool CheckIfValidItem(GameObject entityGameObject, Items.Item item, Player player)
    {
        if (!string.IsNullOrEmpty(TypeName))
        {
            foreach (MoveableEntity ent in entityGameObject.GetComponents<MoveableEntity>())
            {
                if (TypeName.Equals(ent.GetType().Name))
                {
                    //Success, this object has the type we want.
                    if (DoSnap && ent != _ignoredEntity) Snap(player.inventory.DropItem(item, Vector3.zero), ent);
                    return true;
                }
            }
        }
        else if (PlaceZoneID != null)
        {
            ItemPlaceZoneID id = entityGameObject.GetComponent<ItemPlaceZoneID>();
            if (id && id.ID == PlaceZoneID)
            {
                //This matches the id. Get the moveable entity and use that for snapping.
                MoveableEntity ent = entityGameObject.GetComponent<MoveableEntity>();
                if (ent)
                {
                    if (DoSnap && ent != _ignoredEntity) Snap(player.inventory.DropItem(item, Vector3.zero), ent);
                    return true;
                }
            }
        }
        return false;
    }
}

[Serializable]
public class ItemPlaceZoneEvent : UnityEvent<ItemPlaceZone, MoveableEntity>
{

}
