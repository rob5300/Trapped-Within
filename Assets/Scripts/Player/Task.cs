using System;
using UnityEngine;

[Serializable]
public class Task
{
    public string Name;
    public string Description;
    [HideInInspector]
    public bool Completed = false;

    public Task(string name, string description)
    {
        Name = name;
        Description = description;
    }

}

