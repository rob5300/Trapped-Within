using cakeslice;
using Entity;
using Items;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInteraction : MonoBehaviour
{
    public bool InteractEvent = true;
    public bool MoveEntitiys = true;
    public float VelocityRatio;
    public float ReachDistance = 5;
    public bool DrawOutline = true;
    public float OutlineDelay = 1.2f;

    private Player player;
    private Ray interactionRay;
    private RaycastHit interactHit;
    private RaycastHit itemInteractHit;

    private MoveableEntity _grabbedEntity;
    private RigidbodyInterpolation _oldEntityMode;
    private float _oldEntityAngularDrag;
    private Rigidbody _entityRB;
    private Vector3 _newMovePoint;
    private Vector3 _offset;
    private float _velocityClamp = 10;
    private Vector3 _newVelocity;
    private float _timeOnObject;
    private GameObject _lookAtObject;
    private bool _lookatActive = false;

    public void Awake()
    {
        player = GetComponent<Player>();
        player.camera.GetComponent<OutlineEffect>().enabled = DrawOutline;
    }

    public void Update()
    {
        InputChecks();
        LookAtCheck();
    }

    public void FixedUpdate()
    {
        //Update the position of the grabbed entity
        if (_grabbedEntity != null)
        {
            if (_grabbedEntity.Moveable)
            {
                _newMovePoint = player.camera.transform.TransformPoint(_offset);
                _newVelocity = (_newMovePoint - _entityRB.transform.position) * VelocityRatio;
                _newVelocity = new Vector3(Mathf.Clamp(_newVelocity.x, -_velocityClamp, _velocityClamp), Mathf.Clamp(_newVelocity.y, -_velocityClamp, _velocityClamp), Mathf.Clamp(_newVelocity.z, -_velocityClamp, _velocityClamp));
                _entityRB.velocity = _newVelocity;
            }
            else
            {
                //TODO: Have the object be dropped if its no longer moveable.
            }
        }
        
    }

    public void LookAtCheck()
    {
        interactionRay = new Ray(player.camera.transform.position, player.camera.transform.forward);
        if (Physics.Raycast(interactionRay, out interactHit, ReachDistance))
        {
            MoveableEntity moveableEntity =
                interactHit.transform.GetComponent<MoveableEntity>() ??
                interactHit.transform.GetComponentInParent<MoveableEntity>();
            IInteractable interactable =
                interactHit.transform.GetComponent<IInteractable>() ??
                interactHit.transform.GetComponentInParent<IInteractable>();
            IItemInteract itemInteract =
                interactHit.transform.GetComponent<IItemInteract>() ??
                interactHit.transform.GetComponentInParent<IItemInteract>();
            if (moveableEntity != null)
            {
                //If this a moveable entity.
                Ui.SetMoveableEntityVisibility(true);

                if (_lookAtObject == moveableEntity.gameObject && DrawOutline)
                {
                    _timeOnObject += Time.deltaTime;
                }
                else
                {
                    _timeOnObject = 0;
                    _lookatActive = false;

                    //If we already have an object, we remove its outline component.
                    if (_lookAtObject)
                    {
                        Outline outL = _lookAtObject.GetComponent<Outline>();
                        if (outL)
                        {
                            Destroy(outL);
                        } 
                    }
                    //We assign the new object after.
                    _lookAtObject = moveableEntity.gameObject;
                }
            }
            else if (interactable != null)
            {
                //If this is interactable
                if (_lookAtObject == ((MonoBehaviour) interactable).gameObject && DrawOutline)
                {
                    _timeOnObject += Time.deltaTime;
                }
                else
                {
                    _timeOnObject = 0;
                    _lookatActive = false;

                    //If we already have an object, we remove its outline component.
                    if (_lookAtObject)
                    {
                        Outline outL = _lookAtObject.GetComponent<Outline>();
                        if (outL)
                        {
                            Destroy(outL);
                        }
                    }
                    //We assign the new object after.
                    _lookAtObject = ((MonoBehaviour) interactable).gameObject;
                }
            }
            
            //This object is item interactable. Enable crosshair only if we have an item equipped.
            UIMonoHelper.Instance.InteractableCrosshair.SetActive(((interactable != null && interactable.Interactable) || (itemInteract != null && player.inventory.EquippedItem != null)));

            //Is another object type, remove the effect.
            if (_lookAtObject != null && moveableEntity == null && interactable == null)
            {
                Outline outL = _lookAtObject.GetComponent<Outline>();
                UIMonoHelper.Instance.InteractableCrosshair.SetActive(false);
                if (outL)
                {
                    Destroy(outL);
                }
                _timeOnObject = 0;
                _lookAtObject = null;
                _lookatActive = false;
                Ui.SetMoveableEntityVisibility(false);
            }

            //Timer has been met, add outline.
            if (_timeOnObject > OutlineDelay && !_lookatActive && DrawOutline)
            {
                _lookAtObject.AddComponent<Outline>();
                _lookatActive = true;
            }
        }
        else
        {
            //Raycast hit nothing, reset and remove.
            _lookatActive = false;
            _timeOnObject = 0;
            UIMonoHelper.Instance.InteractableCrosshair.SetActive(false);
            if (_lookAtObject)
            {
                Outline outL = _lookAtObject.GetComponent<Outline>();
                if (outL)
                {
                    Destroy(outL);
                }
                _timeOnObject = 0;
                _lookAtObject = null;
                _lookatActive = false;
                Ui.SetMoveableEntityVisibility(false);
            }
        }
    }

    public void InputChecks()
    {
        if (!Ui.InventoryVisible && !Ui.PauseMenuVisible)
        {
            //Only do these inputs if we dont have a window open.
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
                if (player.inventory.EquippedItem != null) ItemInteract(player.inventory.GetEquippedItem());
            } 
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Ui.ToggleInventory();
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Ui.Escape();
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            player.movement.Croutch();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            player.movement.TryUnCroutch();
        }
        //Entity Moving
        MoveEntity(Input.GetMouseButton(1));
    }

    void Interact()
    {
        interactionRay = new Ray(player.camera.transform.position, player.camera.transform.forward);
        if (Physics.Raycast(interactionRay, out interactHit, ReachDistance))
        {
            IInteractable obHit =
                interactHit.transform.GetComponent<IInteractable>() ??
                interactHit.transform.GetComponentInParent<IInteractable>();
            if (obHit != null)
            {
                if (obHit.Interactable)
                {
                    obHit.OnInteract(player);
                    MoveableEntity ent = obHit as MoveableEntity;
                    if(ent && ent.SnapZone != null) ent.OnUnsnap(false);
                }
            }
        }
    }

    void ItemInteract(Items.Item item)
    {
        interactionRay = new Ray(player.camera.transform.position, player.camera.transform.forward);
        if (Physics.Raycast(interactionRay, out itemInteractHit, ReachDistance, Physics.DefaultRaycastLayers , QueryTriggerInteraction.Collide))
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
                interactionRay = new Ray(player.camera.transform.position, player.camera.transform.forward);
                if (Physics.Raycast(interactionRay, out itemInteractHit, ReachDistance, Physics.DefaultRaycastLayers,
                    QueryTriggerInteraction.Ignore))
                {
                    _grabbedEntity =
                        itemInteractHit.transform.GetComponent<MoveableEntity>() ??
                        itemInteractHit.transform.GetComponentInParent<MoveableEntity>();
                    if (_grabbedEntity != null && _grabbedEntity.Moveable)
                    {
                        _offset = player.camera.transform.InverseTransformPoint(_grabbedEntity.transform.position);
                        _entityRB = _grabbedEntity.GetComponent<Rigidbody>();
                        if(!_entityRB) _entityRB = _grabbedEntity.GetComponentInChildren<Rigidbody>();
                        _oldEntityMode = _entityRB.interpolation;
                        _entityRB.interpolation = RigidbodyInterpolation.Extrapolate;
                        _oldEntityAngularDrag = _entityRB.angularDrag;
                        _entityRB.angularDrag = 10;

                        _grabbedEntity.OnEntityMoved(player);
                        if (_grabbedEntity.SnapZone)
                        {
                            _grabbedEntity.OnUnsnap();
                        }
                    }
                }
            }
        }
        else
        {
            if(_grabbedEntity) _grabbedEntity = null;
            if (_entityRB)
            {
                _entityRB.interpolation = _oldEntityMode;
                _entityRB.angularDrag = _oldEntityAngularDrag;
                _entityRB = null; 
            }
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
