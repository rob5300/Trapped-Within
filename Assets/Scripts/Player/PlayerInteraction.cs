using Entity;
using Items;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInteraction : MonoBehaviour
{

    public bool InteractEvent = true;

    private Player player;
    private Ray interactRay;
    private Ray itemInteractRay;
    private RaycastHit interactHit;
    private RaycastHit itemInteractHit;

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
            //TODO: drop items.
            Debug.Log("Drop item not working");
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

}
