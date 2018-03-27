using Entity;
using UnityEngine;

[RequireComponent(typeof(Door))]
public class DoorCloser : MonoBehaviour {

    public float Distance = 6f;
    public Door door;

    public void Awake()
    {
        enabled = false;
    }

    public void Update()
    {
        if(Vector3.Distance(Player.instance.transform.position, transform.position) > Distance)
        {
            door.CloseDoor();
            enabled = false;
        }
    }

}
