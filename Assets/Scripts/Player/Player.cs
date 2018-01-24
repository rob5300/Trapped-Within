using Items;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player instance;

    public Camera camera;
    public Transform HoldPoint;
    [HideInInspector]
    public GameObject HeldObject;
    public PlayerMovement movement;
    public Inventory inventory;
    public TaskManager TaskManager;

    public Player()
    {
        instance = this;
        inventory = new Inventory(8, this);
    }

    public void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }

    public Vector3 GetDropPosition()
    {
        Vector3 dropPoint;

        //If there is an object within 4 units foward from the camera, we will drop the item there instead.
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 1.5f))
        {
            //We make the drop point a bit back from the hit point to avoid the object being lost within the object.
            dropPoint = Vector3.Lerp(Camera.main.transform.position, hit.point, 0.9f);
        }
        //There was no object within 4 units, we simply drop at 4 units away.
        else
        {
            dropPoint = Camera.main.transform.position + Camera.main.transform.forward * 1.5f;
        }
        return dropPoint;
    }
}
