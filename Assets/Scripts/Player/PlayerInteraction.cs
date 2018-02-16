using Entity;
using Items;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInteraction : MonoBehaviour
{

    public bool InteractEvent = true;
    public bool MoveEntitiys = true;
    public float VelocityRatio;

    private Player player;
    private Ray interactRay;
    private Ray itemInteractRay;
    private RaycastHit interactHit;
    private RaycastHit itemInteractHit;

    private MoveableEntity _grabbedEntity;
    private RigidbodyInterpolation oldEntityMode;
    private Rigidbody entityRB;
    private Vector3 _newMovePoint;
    private Vector3 _offset;
    private float _velocityClamp = 10;
    private Vector3 _newVelocity;

    public void Start()
    {
        player = GetComponent<Player>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropHeldItem();
        }
        if (Input.GetMouseButtonDown(0))
        {
            if(player.inventory.EquippedItem != null) ItemInteract(player.inventory.GetEquippedItem());
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Ui.ToggleInventory();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Ui.Escape();
        }

        //Entity Moving
        MoveEntity(Input.GetMouseButton(1));
    }

    public void FixedUpdate()
    {
        if (_grabbedEntity != null)
        {
            _newMovePoint = player.camera.transform.TransformPoint(_offset);
            _newVelocity = (_newMovePoint - _grabbedEntity.transform.position) * VelocityRatio;
            _newVelocity = new Vector3(Mathf.Clamp(_newVelocity.x, -_velocityClamp, _velocityClamp), Mathf.Clamp(_newVelocity.y, -_velocityClamp, _velocityClamp), Mathf.Clamp(_newVelocity.z, -_velocityClamp, _velocityClamp));
            entityRB.velocity = _newVelocity;
        }
    }

    void Interact()
    {
        interactRay = new Ray(player.camera.transform.position, player.camera.transform.forward);
        if (Physics.Raycast(interactRay, out interactHit, 5f))
        {
            IInteractable obHit =
                interactHit.transform.GetComponent<IInteractable>() ??
                interactHit.transform.GetComponentInParent<IInteractable>();
            if (obHit != null)
            {
                if (obHit.Interactable) obHit.OnInteract(player);
            }
        }
    }

    void ItemInteract(Items.Item item)
    {
        itemInteractRay = new Ray(player.camera.transform.position, player.camera.transform.forward);
        if (Physics.Raycast(itemInteractRay, out itemInteractHit, 5f, Physics.DefaultRaycastLayers , QueryTriggerInteraction.Collide))
        {
            IItemInteract obHit =
                itemInteractHit.transform.GetComponent<IItemInteract>() ??
                itemInteractHit.transform.GetComponentInParent<IItemInteract>();
            if (obHit != null)
            {
                //We tell the inventory that the equipped item was used, and we pass through the return value from the event call.
                //The event returns true if the interaction was successful. This avoids false triggers if an incorrect item was used on an object.
                player.inventory.EquipedItemUsed(player.HeldObject, obHit.OnItemInteract(item, player));
            }
        }
    }

    void MoveEntity(bool shouldMove)
    {
        if (shouldMove)
        {
            if (MoveEntitiys && !_grabbedEntity)
            {
                itemInteractRay = new Ray(player.camera.transform.position, player.camera.transform.forward);
                if (Physics.Raycast(itemInteractRay, out itemInteractHit, 5f, Physics.DefaultRaycastLayers,
                    QueryTriggerInteraction.Ignore))
                {
                    _grabbedEntity =
                        itemInteractHit.transform.GetComponent<MoveableEntity>() ??
                        itemInteractHit.transform.GetComponentInParent<MoveableEntity>();
                    if (_grabbedEntity != null)
                    {
                        _offset = player.camera.transform.InverseTransformPoint(_grabbedEntity.transform.position);
                        entityRB = _grabbedEntity.GetComponent<Rigidbody>();
                        oldEntityMode = entityRB.interpolation;
                        entityRB.interpolation = RigidbodyInterpolation.Extrapolate;
                    }
                }
            }
        }
        else
        {
            _grabbedEntity = null;
            entityRB.interpolation = oldEntityMode;
            entityRB = null;
        }
    }

    void DropHeldItem()
    {
        if (player.inventory.EquippedItem != null)
        {
            player.inventory.DropItem(player.inventory.GetEquippedItem(), player.GetDropPosition());
        }
    }
}
