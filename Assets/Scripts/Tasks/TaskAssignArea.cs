using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TaskAssignArea : MonoBehaviour
{
    public Task TaskToAssign;

    public void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<Player>())
        {
            Player.instance.TaskManager.SetCurrentTask(TaskToAssign);
            Destroy(this); 
        }
    }

    public void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }
}
